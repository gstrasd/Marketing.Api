using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Marketing.Application.Domain;
using Marketing.Infrastructure.Domain;
using Marketing.Infrastructure.Domain.Entities;
using Serilog;

namespace Marketing.Application
{
    internal class LeadService : ILeadService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly ICollectionRepository _collectionRepository;
        private readonly IIntakeRepository _intakeRepository;
        private readonly SessionManager _sessionManager;
        private readonly Settings _settings;
        private readonly AsyncLocal<int?> _intakeId;
        private readonly ILogger _logger;

        public LeadService(IPatientRepository patientRepository, ICollectionRepository collectionRepository, IIntakeRepository intakeRepository, SessionManager sessionManager, Settings settings, AsyncLocal<int?> intakeId, ILogger logger)
        {
            _patientRepository = patientRepository;
            _collectionRepository = collectionRepository;
            _intakeRepository = intakeRepository;
            _sessionManager = sessionManager;
            _settings = settings;
            _intakeId = intakeId;
            _logger = logger;
        }

        public async Task SubmitLeadAsync(Lead lead)
        {
            // Begin Lazarus session
            await using var session = _sessionManager.BeginAsync();

            var patient = await _patientRepository.FindPatientAsync(lead.FirstName, lead.LastName, lead.PhoneNumber);
            if (patient == null)
            {
                // Create lead for new patient
                var collectionId = await _collectionRepository.CreateLeadAsync(lead);
                await _intakeRepository.MarkIntakeSuccessful(_intakeId.Value!.Value, collectionId);
                _logger.Information($"A new {lead.Source} lead was received from a new patient and a corresponding order was created.");
                return;
            }

            var (order, item) = await _collectionRepository.FindLeadAsync(patient.PersonId);

            switch (order)
            {
                // Create new lead for existing patient 
                case null:
                    var collectionId = await _collectionRepository.CreateLeadAsync(lead, patient);
                    await _intakeRepository.MarkIntakeSuccessful(_intakeId.Value!.Value, collectionId);
                    _logger.Information($"A new {lead.Source} lead was received from an existing patient and a corresponding order was created.");
                    return;

                // Existing lead is already to be worked on. Do nothing.
                case { Status: Status.Active }:
                    await _collectionRepository.AddNoteAsync(order.CollectionId, "System Generated Message", $"{lead.FirstName} {lead.LastName} has submitted another {lead.Source} lead for a {lead.Interest.ToLower()}.");
                    _logger.Information($"A new {lead.Source} lead was received; however, an existing lead already exists that is currently being worked on.");
                    break;

                // Patient has submitted another lead. Add a note.
                case { Flow: Flow.InsideLeads, Status: Status.Ready }:
                    var requested = _settings.LeadItems.FirstOrDefault(li => li.Product.Equals(lead.Interest, StringComparison.OrdinalIgnoreCase))?.ProductId;
                    var existing = item!.ProductId;
                    var note = requested == existing
                        ? $"{lead.FirstName} {lead.LastName} has submitted another {lead.Source} lead for a {lead.Interest.ToLower()}."
                        : $"{lead.FirstName} {lead.LastName} has submitted another {lead.Source} lead. This time expressing interest in a {lead.Interest.ToLower()}.";
                    await _collectionRepository.AddNoteAsync(order.CollectionId, "System Generated Message", note);
                    _logger.Information($"A new {lead.Source} lead was received; however, an existing lead already exists.");
                    break;

                // Renew existing lead that was placed on hold
                case { Flow: Flow.InsideLeads, Status: Status.Hold }:
                    await _collectionRepository.CheckoutAsync(order.CollectionId, Flow.InsideLeads);
                    await _collectionRepository.AddNoteAsync(order.CollectionId, "System Generated Message", $"{lead.FirstName} {lead.LastName} has submitted another {lead.Source} lead for a {lead.Interest.ToLower()}. Please contact patient again.");
                    await _collectionRepository.CheckInAsync(order.CollectionId, Flow.InsideLeads, _settings.Outcomes["RenewHeldLead"]);
                    _logger.Information($"A new {lead.Source} lead was received and an existing lead was found that is currently on hold. It has been renewed and made ready to be worked on again.");
                    break;

                // Existing lead is already being processed. Add a note.
                case { Flow: Flow.BusinessIntelligence }:
                    await _collectionRepository.AddNoteAsync(order.CollectionId, "System Generated Message", $"{lead.FirstName} {lead.LastName} has submitted another {lead.Source} lead for a {lead.Interest.ToLower()}.");
                    _logger.Information($"A new {lead.Source} lead was received; however, an existing lead is already being processed.");
                    break;

                // Renew existing lead that was previously completed
                case { Flow: Flow.OrderComplete }:
                    // TODO: not moving to 1200
                    await _collectionRepository.CheckoutAsync(order.CollectionId, Flow.OrderComplete);
                    await _collectionRepository.AddNoteAsync(order.CollectionId, "System Generated Message", $"{lead.FirstName} {lead.LastName} has submitted another {lead.Source} lead for a {lead.Interest.ToLower()}. Please contact patient again.");
                    await _collectionRepository.CheckInAsync(order.CollectionId, Flow.InsideLeads, _settings.Outcomes["RenewCompletedLead"]);
                    _logger.Information($"A new {lead.Source} lead was received and an existing lead was found that was already completed. It has been renewed and made ready to be worked on again.");
                    break;
            }

            await _intakeRepository.MarkIntakeSuccessful(_intakeId.Value!.Value, order.CollectionId);
        }
    }
}
