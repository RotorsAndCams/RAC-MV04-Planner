using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MV04.Camera
{
    public class CameraSettingsManager
    {
        public async static void OpenCameraSettingsForm()
        {
            using (var form = new CameraSettingsForm())
            {
                form.ShowDialog();
            }
        }

    }
}
