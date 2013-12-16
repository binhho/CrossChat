using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Abo.Client.Core.Managers;
using Abo.Client.WP.Silverlight.Converters;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Abo.Client.WP.Silverlight.ViewModels
{
    public class PhotoSelectorViewModel : BaseViewModel
    {
        private readonly AccountManager _accountManager;

        public PhotoSelectorViewModel(AccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public ICommand SelectBuiltinPhotoCommand
        {
            get { return new RelayCommand<int>(SelectBuiltinPhoto); }
        }

        public int[] BuiltinPhotos
        {
            get { return Enumerable.Range(1, IdToPhotoUrlConverter.LastFaceIndex).Select(i => i * -1).ToArray(); }
        }

        private async void SelectBuiltinPhoto(int id)
        {
            IsBusy = true;
            var result = await _accountManager.ChangePhotoToBuiltin(id);
            IsBusy = false;
            if (!result)
            {
                MessageBox.Show("Failed to change the photo ;(");
            }
            else
            {
                RequestClose();
            }
        }

        public Task<PhotoSelectionModeEnum> SelectMode()
        {
            var taskSource = new TaskCompletionSource<PhotoSelectionModeEnum>();
            var messageBox = new CustomMessageBox
                {
                    Caption = "Photo selection",
                    Message = "Specify the source of the photos please.",
                    LeftButtonContent = "My photos",
                    RightButtonContent = "Astral photos"
                };
            messageBox.Dismissed += (s, e) =>
                {
                    switch (e.Result)
                    {
                        case CustomMessageBoxResult.LeftButton:
                            taskSource.TrySetResult(PhotoSelectionModeEnum.MyPhotos);
                            break;
                        case CustomMessageBoxResult.RightButton:
                            taskSource.TrySetResult(PhotoSelectionModeEnum.BuiltinPhotos);
                            break;
                        default:
                            taskSource.TrySetResult(PhotoSelectionModeEnum.None);
                            break;
                    }
                };
            messageBox.Show();
            return taskSource.Task;
        }

        public async void SelectPhoto(bool onlyBuiltin)
        {
            if (onlyBuiltin)
            {
                Show();
                return;
            }

            var mode = await SelectMode();
            if (mode == PhotoSelectionModeEnum.None)
                return;

            if (mode == PhotoSelectionModeEnum.BuiltinPhotos)
            {
                Show();
                return;
            }
            
            var task = new PhotoChooserTask();
            task.Completed += task_OnCompleted;
            task.PixelHeight = 220;
            task.PixelWidth = 220;
            task.ShowCamera = true;
            task.Show();
        }

        private async void task_OnCompleted(object sender, PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK || e.ChosenPhoto == null)
                return;

            byte[] array;
            using (var ms = new MemoryStream())
            {
                e.ChosenPhoto.CopyTo(ms);
                ms.Position = 0;
                array = ms.ToArray();
            }
            IsBusy = true;
            var result = await _accountManager.ChangePhoto(array);
            IsBusy = false;
            if (!result)
            {
                MessageBox.Show("Failed to upload the photo ;(");
            }
        }

        public enum PhotoSelectionModeEnum
        {
            None,
            MyPhotos,
            BuiltinPhotos
        }
    }
}
