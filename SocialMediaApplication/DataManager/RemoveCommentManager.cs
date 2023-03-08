﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialMediaApplication.Database.DatabaseHandler.Contract;
using SocialMediaApplication.Database.DatabaseHandler;
using SocialMediaApplication.DataManager.Contract;
using SocialMediaApplication.Domain.UseCase;
using SocialMediaApplication.Models.BusinessModels;
using SocialMediaApplication.Models.EntityModels;
using SocialMediaApplication.Util;
using System.Xml.Linq;

namespace SocialMediaApplication.DataManager
{
    public sealed class RemoveCommentManager : IRemoveCommentManager
    {
        private static RemoveCommentManager Instance { get; set; }
        private static readonly object PadLock = new object();

        private RemoveCommentManager() { }

        public static RemoveCommentManager GetInstance
        {
            get
            {
                if (Instance == null)
                {
                    lock (PadLock)
                    {
                        if (Instance == null)
                        {
                            Instance = new RemoveCommentManager();
                        }
                    }
                }
                return Instance;
            }
        }
        private readonly ICommentDbHandler _commentDbHandler = CommentDbHandler.GetInstance;
        private readonly IReactionManager _reactionManager = ReactionManager.GetInstance;
        private readonly IUserDbHandler _userDbHandler= UserDbHandler.GetInstance;

        public static event Action CommentRemoved;


        public async Task RemoveCommentAsync(RemoveCommentRequest removeCommentRequest,
            RemoveCommentUseCaseCallBack removeCommentUseCaseCallBack)
        {
            try
            {
                var comments = (await _commentDbHandler.GetPostCommentsAsync(removeCommentRequest.Comment.PostId)).ToList();
                var (childComments,removedCommentIdList) = RemovableCommentList(comments, removeCommentRequest.Comment.Id);
                var commentBObjList =  await AddCommentManager.GetInstance.GetSortedCommentBObjList(comments);
                foreach (var comment in childComments)
                {
                    var user = await _userDbHandler.GetUserAsync(AppSettings.UserId);
                    await _commentDbHandler.RemoveCommentAsync(comment.Id);
                    var commentBObj = commentBObjList.First(c => c.Id == comment.Id);
                    commentBObjList.Remove(commentBObj);
                }
                await _commentDbHandler.RemoveCommentAsync(removeCommentRequest.Comment.Id);
                var commentBusinessObj = commentBObjList.First(c => c.Id == removeCommentRequest.Comment.Id);
                commentBObjList.Remove(commentBusinessObj);

                CommentRemoved?.Invoke();

                removeCommentUseCaseCallBack?.OnSuccess(new RemoveCommentResponse());
            }
            catch (Exception e)
            {
                removeCommentUseCaseCallBack?.OnError(e);
            }
        }

        private Tuple<List<Comment>,List<String>> RemovableCommentList(List<Comment> comments, string commentId)
        {
            List<Comment> commentList = new List<Comment>();
            List<string> commentIdList = new List<string>();

            foreach (var c in comments)
            {
                if (c.ParentCommentId == commentId)
                {
                    commentList.Add(c);
                    commentIdList.Add(c.Id);
                    var childOfChild = comments.Where(cc => cc.ParentCommentId == c.Id);
                    if (childOfChild.Any())
                    {
                        var (childComment,removedCommentListId) = RemovableCommentList(comments, c.Id);
                        commentList.AddRange(childComment);
                        commentIdList.AddRange(removedCommentListId);
                    }
                }

            }
            return Tuple.Create(commentList,commentIdList);
        }

        public Comment ConvertCommentBObjToEntity(CommentBObj commentBobj)
        {
            Comment comment = new Comment();
            comment.Id = commentBobj.Id;
            comment.ParentCommentId = commentBobj.ParentCommentId;
            comment.CommentedBy = commentBobj.CommentedBy;
            comment.CommentedAt = commentBobj.CommentedAt;
            comment.Content = commentBobj.Content;
            comment.PostId = commentBobj.PostId;

            return comment;
        }
    }
}
