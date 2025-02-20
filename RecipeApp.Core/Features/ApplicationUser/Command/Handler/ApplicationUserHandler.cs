using AutoMapper;
using MediatR;
using RecipeApp.Core.Features.ApplicationUser.Command.Model;
using RecipeApp.Service.Abstraction;
using RecipeApp.Shared.Bases;

namespace RecipeApp.Core.Features.ApplicationUser.Command.Handler
{
    internal class ApplicationUserHandler :
        IRequestHandler<AddApplicationUserCommand, ReturnBase<string>>,
        IRequestHandler<ConfirmEmailCommand, ReturnBase<bool>>
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IConfirmEmailSerivce _confirmEmailSerivce;
        private readonly IMapper _mapper;
        public ApplicationUserHandler(IApplicationUserService applicationUserService, IMapper mapper, IConfirmEmailSerivce confirmEmailSerivce)
        {
            _applicationUserService = applicationUserService;
            _mapper = mapper;
            _confirmEmailSerivce = confirmEmailSerivce;
        }
        public async Task<ReturnBase<string>> Handle(AddApplicationUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Domain.Entities.Identity.ApplicationUser mappedResult = _mapper.Map<Domain.Entities.Identity.ApplicationUser>(request);

                ReturnBase<string> createUserResult = await _applicationUserService.AddApplicationUserAsync(mappedResult, request.Password);

                if (createUserResult.Succeeded)
                {
                    return ReturnBaseHandler.Created("", createUserResult.Message);
                }
                return ReturnBaseHandler.Failed<string>("Failed to add user.");
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<string>($"{ex.Message}");
            }
        }
        public async Task<ReturnBase<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReturnBase<bool> confrimEmailResult = await _confirmEmailSerivce.ConfirmEmailAsync(request.UserId, request.Token);

                if (confrimEmailResult.Succeeded)
                {
                    return ReturnBaseHandler.Created(true, confrimEmailResult.Message);
                }
                return ReturnBaseHandler.Failed<bool>(confrimEmailResult.Message);
            }
            catch (Exception ex)
            {
                return ReturnBaseHandler.Failed<bool>(ex.Message);
            }
        }
    }
}