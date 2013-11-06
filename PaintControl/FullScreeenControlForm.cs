using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Painter
{
    public partial class FullScreeenControlForm : Form
    {
        public event EventHandler Escaped;
        public FullScreeenControlForm()
        {
            InitializeComponent();
        }

        private void FullScreeenControlForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
                if (Escaped != null) Escaped.Invoke(sender, new EventArgs());
            }
        }
    }
}
