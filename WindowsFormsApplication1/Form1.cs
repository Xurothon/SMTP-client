using System;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net.Security;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Connection connect = new Connection();
            connect.connnection();
            connect.authLogin(txtUsername.Text, txtPassword.Text);
            connect.makeMail(txtTo.Text);
            connect.sendMail(txtMessage.Text);
            connect.closeConnection();
            richTextBox1.Text = Addition.getNeedStr();
        }

    }

    public class Addition
    {
        private static string needStr = "Диалог с сервером";
        private Addition() { }

        public static void textS(string str)
        {
            needStr += "\nS: " + str;
        }

        public static void textC(string str)
        {
            needStr += "\nC: " + str;
        }

        public static string getNeedStr()
        {
            return needStr;
        }
    }

    public class Connection
    {
        private TcpClient client;
        private SslStream sslStream;
        private byte[] buffer = new byte[2048];
        int bytes = -1;

        public Connection()
        {
            this.client = new TcpClient();
            client.Connect("smtp.gmail.com", 465);
            this.sslStream = new SslStream(client.GetStream());
        }

        public void connnection()
        {
            
            sslStream.AuthenticateAsClient("smtp.gmail.com");
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
            sslStream.Write(Encoding.UTF8.GetBytes("EHLO gmail.com" + "\r\n"));
            Addition.textC("EHLO gmail.com");
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
        }

        public void authLogin(string userName, string password)
        {
            sslStream.Write(Encoding.UTF8.GetBytes("AUTH LOGIN" + "\r\n"));
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textC("AUTH LOGIN");
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
            string mail = Convert.ToBase64String(Encoding.UTF8.GetBytes(userName));
            byte[] mail2 = Encoding.UTF8.GetBytes(mail + "\r\n");
            sslStream.Write(mail2);
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textC(Convert.ToBase64String(Encoding.UTF8.GetBytes(userName)));
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
            mail = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            mail2 = Encoding.UTF8.GetBytes(mail + "\r\n");
            sslStream.Write(mail2);
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textC(Convert.ToBase64String(Encoding.UTF8.GetBytes(password)));
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
        }

        public void makeMail(string mailTo)
        {
            sslStream.Write(Encoding.UTF8.GetBytes("MAIL FROM: " + "<>" + "\r\n"));
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textC("MAIL FROM: " + "<>");
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
            sslStream.Write(Encoding.UTF8.GetBytes("RCPT TO: " + "<" + mailTo + ">" + "\r\n"));
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textC("RCPT TO: " + "<" + mailTo + ">");
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
        }

        public void sendMail(string message)
        {
            sslStream.Write(Encoding.UTF8.GetBytes("DATA" + "\r\n"));
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textC("DATA");
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
            sslStream.Write(Encoding.UTF8.GetBytes(message + "\n.\n"));
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textC(message);
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));            
        }

        public void closeConnection()
        {
            sslStream.Write(Encoding.UTF8.GetBytes("QUIT" + "\r\n"));
            bytes = sslStream.Read(buffer, 0, buffer.Length);
            Addition.textC("QUIT");
            Addition.textS(Encoding.UTF8.GetString(buffer, 0, bytes));
        }
        
    }
}
