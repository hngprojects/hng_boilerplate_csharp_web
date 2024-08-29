using Hng.Application.Features.ContactsUs.Command;
using Hng.Application.Features.ContactsUs.Dtos;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using MediatR;

namespace Hng.Application.Features.ContactsUs.Handlers
{
    public class DeleteContactUsCommandHandler : IRequestHandler<DeleteContactUsCommand, ContactResponse<object>>
    {
        private readonly IRepository<ContactUs> _repository;

        public DeleteContactUsCommandHandler(IRepository<ContactUs> repository)
        {
            _repository = repository;
        }

        public async Task<ContactResponse<object>> Handle(DeleteContactUsCommand request, CancellationToken cancellationToken)
        {
            var contact = await _repository.GetAsync(request.Id);
            if (contact == null)
            {
                return new ContactResponse<object>
                {
                    StatusCode = 404,
                    Message = "ContactUs not found"
                };

            }
            await _repository.DeleteAsync(contact);
            await _repository.SaveChanges();
            return new ContactResponse<object>
            {
                StatusCode = 200,
                Message = "ContactUs deleted successfully"
            };
        }
    }
}
