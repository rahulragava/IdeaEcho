﻿using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.DataManager;
using SocialMediaApplication.Presenter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApplication.Domain.UseCase
{
   public class EditHomeImageUseCase : UseCaseBase<EditHomeImageResponse>
    {
        private readonly IEditProfileImageManager _editProfileImageManager = EditProfileImageManager.GetInstance;
        public EditHomeImageRequest EditHomeImageRequest;

        public EditHomeImageUseCase(EditHomeImageRequest editHomeImageRequest)
        {
            EditHomeImageRequest = editHomeImageRequest;
        }

        public override void Action()
        {
            _editProfileImageManager.EditHomeImageAsync(EditHomeImageRequest,
                new EditHomeImageUseCaseCallBack(this));
        }
    }


    //req obj
    public class EditHomeImageRequest
    {
        public EditHomeImagePresenterCallBack EditHomeImagePresenterCallBack { get; }
        public string UserId { get; }
        public string ImagePath { get; }

        public EditHomeImageRequest(string userId, string imagePath, EditHomeImagePresenterCallBack editHomeImagePresenterCallBack)
        {
            UserId = userId;
            ImagePath = imagePath;
            EditHomeImagePresenterCallBack = editHomeImagePresenterCallBack;
        }
    }

    //response obj
    public class EditHomeImageResponse
    {
        public bool Success { get; }
        public EditHomeImageResponse(bool success)
        {
            Success = success;
        }
    }

    public class EditHomeImageUseCaseCallBack : IUseCaseCallBack<EditHomeImageResponse>
    {
        private readonly EditHomeImageUseCase _editHomeImageUseCase;

        public EditHomeImageUseCaseCallBack(EditHomeImageUseCase editProfileImageUseCase)
        {
            _editHomeImageUseCase = editProfileImageUseCase;
        }

        public void OnSuccess(EditHomeImageResponse responseObj)
        {
            _editHomeImageUseCase?.EditHomeImageRequest.EditHomeImagePresenterCallBack.OnSuccess(responseObj);
        }

        public void OnError(Exception ex)
        {
            _editHomeImageUseCase?.EditHomeImageRequest.EditHomeImagePresenterCallBack.OnError(ex);
        }
    }
}
