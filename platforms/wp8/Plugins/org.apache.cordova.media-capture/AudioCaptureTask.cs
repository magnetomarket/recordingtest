﻿/*  
	Licensed under the Apache License, Version 2.0 (the "License");
	you may not use this file except in compliance with the License.
	You may obtain a copy of the License at
	
	http://www.apache.org/licenses/LICENSE-2.0
	
	Unless required by applicable law or agreed to in writing, software
	distributed under the License is distributed on an "AS IS" BASIS,
	WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
	See the License for the specific language governing permissions and
	limitations under the License.
*/

using System;
using System.IO;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace WPCordovaClassLib.Cordova.UI
{
    /// <summary>
    /// Allows an application to launch the Audio Recording application. 
    /// Use this to allow users to record audio from your application.
    /// </summary>
    public class AudioCaptureTask
    {
        /// <summary>
        /// Represents recorded audio returned from a call to the Show method of
        /// a WPCordovaClassLib.Cordova.Controls.AudioCaptureTask object
        /// </summary>
        public class AudioResult : TaskEventArgs
        {
            /// <summary>
            /// Initializes a new instance of the AudioResult class.
            /// </summary>
            public AudioResult()
            { }

            /// <summary>
            /// Initializes a new instance of the AudioResult class
            /// with the specified Microsoft.Phone.Tasks.TaskResult.
            /// </summary>
            /// <param name="taskResult">Associated Microsoft.Phone.Tasks.TaskResult</param>
            public AudioResult(TaskResult taskResult)
                : base(taskResult)
            { }

            /// <summary>
            ///  Gets the file name of the recorded audio.
            /// </summary>
            public Stream AudioFile { get; internal set; }

            /// <summary>
            /// Gets the stream containing the data for the recorded audio.
            /// </summary>
            public string AudioFileName { get; internal set; }
        }

        /// <summary>
        /// Occurs when a audio recording task is completed.
        /// </summary>
        public event EventHandler<AudioResult> Completed;

        /// <summary>
        /// Shows Audio Recording application
        /// </summary>
        public void Show()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                var root = Application.Current.RootVisual as PhoneApplicationFrame;

                root.Navigated += new System.Windows.Navigation.NavigatedEventHandler(NavigationService_Navigated);

                string baseUrl = "/";
                // dummy parameter is used to always open a fresh version
                //root.Navigate(new System.Uri(baseUrl + "Plugins/org.apache.cordova.core.media-capture/AudioRecorder.xaml?dummy=" + Guid.NewGuid().ToString(), UriKind.Relative));
                //org.apache.cordova.media-capture
                //KL 5/26/14
                root.Navigate(new System.Uri(baseUrl + "Plugins/org.apache.cordova.media-capture/AudioRecorder.xaml?dummy=" + Guid.NewGuid().ToString(), UriKind.Relative));
            });
        }

        /// <summary>
        /// Performs additional configuration of the recording application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationService_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (!(e.Content is AudioRecorder)) return;

            (Application.Current.RootVisual as PhoneApplicationFrame).Navigated -= NavigationService_Navigated;

            AudioRecorder audioRecorder = (AudioRecorder)e.Content;

            if (audioRecorder != null)
            {
                audioRecorder.Completed += this.Completed;
            }
            else if (this.Completed != null)
            {
                this.Completed(this, new AudioResult(TaskResult.Cancel));
            }
        }
    }
}
