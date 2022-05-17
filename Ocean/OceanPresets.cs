using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Ocean
{
    public class OceanPresets
    {
        private static OceanPresets instance;
        public static OceanPresets Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new OceanPresets();
                }

                return instance;
            }
        }

        private OceanPresets()
        { }




        [Category("Presets")]
        public Color PrimaryColor
        {
            get;
            set;
        } = Color.FromArgb(121, 82, 179);

        [Category("Presets")]
        public Color SecondaryColor
        {
            get;
            set;
        } = Color.FromArgb(107, 116, 126);


        [Category("Presets")]
        public Color SuccessColor
        {
            get;
            set;
        } = Color.FromArgb(0, 145, 86);


        [Category("Presets")]
        public Color DangerColor
        {
            get;
            set;
        } = Color.FromArgb(234, 0, 26);


        [Category("Presets")]
        public Color WarningColor
        {
            get;
            set;
        } = Color.FromArgb(255, 203, 0);

        [Category("Presets")]
        public Color InfoColor
        {
            get;
            set;
        } = Color.FromArgb(0, 200, 248);

        [Category("Presets")]
        public Color LightColor
        {
            get;
            set;
        } = Color.FromArgb(248, 249, 250);

        [Category("Presets")]
        public Color DarkColor
        {
            get;
            set;
        } = Color.FromArgb(33, 36, 42);

    }
}
