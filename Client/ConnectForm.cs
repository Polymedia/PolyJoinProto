using System;
using System.Windows.Forms;

namespace Client
{
    public partial class ConnectForm : Form
    {
        public ConnectForm()
        {
            InitializeComponent();

            createRadioButton.CheckedChanged += RadioButtonOnCheckedChanged;
            joinRadioButton.CheckedChanged += RadioButtonOnCheckedChanged;

            startButton.Click += ButtonOnClick;
            joinButton.Click += ButtonOnClick;

            conferenceIdTextBox.TextChanged += ConferenceIdTextBoxOnTextChanged;
            serverIPTextBox.TextChanged += ServerIpTextBoxOnTextChanged;

            RadioButtonOnCheckedChanged(null, null);
        }

        public string ServerIp
        {
            get { return serverIPTextBox.Text; }
            set { serverIPTextBox.Text = value; }
        }
        public string ConferenceId { get; set; }
        public string ClientName
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
        }

        private void RadioButtonOnCheckedChanged(object sender, EventArgs eventArgs)
        {
            startButton.Enabled = createRadioButton.Checked;
            conferenceIdLabel.Enabled = joinRadioButton.Checked;
            conferenceIdTextBox.Enabled = joinRadioButton.Checked;
            joinButton.Enabled = joinRadioButton.Checked;

            ServerIpTextBoxOnTextChanged(null, null);

            if (createRadioButton.Checked)
                conferenceIdTextBox.Text = string.Empty;
        }

        private void ButtonOnClick(object sender, EventArgs eventArgs)
        {
            ServerIp = serverIPTextBox.Text;
            ConferenceId = conferenceIdTextBox.Text;

            DialogResult = DialogResult.OK;

            Hide();
        }

        private void ConferenceIdTextBoxOnTextChanged(object sender, EventArgs eventArgs)
        {
            joinButton.Enabled = !string.IsNullOrEmpty(conferenceIdTextBox.Text) && joinRadioButton.Checked;
        }

        private void ServerIpTextBoxOnTextChanged(object sender, EventArgs eventArgs)
        {
            ConferenceIdTextBoxOnTextChanged(null, null);

            startButton.Enabled = !string.IsNullOrEmpty(serverIPTextBox.Text) && createRadioButton.Checked;
        }
    }
}
