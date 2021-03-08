using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace faspi
{
    class PopUp : System.Windows.Forms.ToolStripDropDown
    {
        private System.Windows.Forms.Control _content;
        private System.Windows.Forms.ToolStripControlHost _host;

        public PopUp(System.Windows.Forms.Control content)
        {
            this.AutoSize = false;
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
            this.BackColor = content.BackColor;
            this._content = content;

            this._host = new System.Windows.Forms.ToolStripControlHost(content);

            this.MinimumSize = content.MinimumSize;
            this.MaximumSize = content.Size;

            this.Size = content.Size;

            content.Location = System.Drawing.Point.Empty;

            this.Items.Add(this._host);

        }
    }
}
