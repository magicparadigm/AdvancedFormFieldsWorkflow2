using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;


using ServiceReference1;
using System.Collections;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            primarySignerSection.Visible = true;
            jointSignerSection.Visible = true;

            button.Visible = true;
            uploadButton.InnerText = "Upload";
            button.InnerText = "Submit";

            getTemplates(sender, e);

            docusignFrame.Visible = false;
            docusignFrameIE.Visible = false;
        }

        // Add event handlers for the navigation button on each of the wizard pages 
        PrefillClick.ServerClick += new EventHandler(prefill_Click);
        button.ServerClick += new EventHandler(button_Click);
        uploadButton.ServerClick += new EventHandler(uploadButton_Click);
    }

    protected void prefill_Click(object sender, EventArgs e)
    {
        firstname.Value = "Warren";
        lastname.Value = "Bytendorp";
        email.Value = "magicparadigm@live.com";
        jointFirstname.Value = "Sheila";
        jointLastname.Value = "Struthers";
        jointEmail.Value = "magicparadigm@live.com";
    }

    protected void button_Click(object sender, EventArgs e)
    {
        primarySignerSection.Visible = false;
        jointSignerSection.Visible = false;
        mainForm.Visible = false;
        button.Visible = false;
        createEnvelope();

    }

    protected void uploadButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                String filename = Path.GetFileName(FileUpload1.FileName);
                FileUpload1.SaveAs(Server.MapPath("~/App_Data/") + filename);
                uploadFile.Value = filename;
            }
        }
        catch (Exception ex)
        {
            uploadFile.Value = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
        }
    }

    protected void getTemplates(object sender, EventArgs e)
    {
        String userName = ConfigurationManager.AppSettings["API.Email"];
        String password = ConfigurationManager.AppSettings["API.Password"];
        String integratorKey = ConfigurationManager.AppSettings["API.IntegratorKey"];
        String accountID = ConfigurationManager.AppSettings["API.AccountID"];

        try
        {
            ServiceReference1.DSAPIServiceSoapClient srv = new ServiceReference1.DSAPIServiceSoapClient();
            String auth = "<DocuSignCredentials><Username>" + userName
                + "</Username><Password>" + password
                + "</Password><IntegratorKey>" + integratorKey
                + "</IntegratorKey></DocuSignCredentials>";


            using (OperationContextScope scope = new System.ServiceModel.OperationContextScope(srv.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers.Add("X-DocuSign-Authentication", auth);
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                EnvelopeTemplateDefinition[] templates = srv.RequestTemplates(accountID, true);
                {

                    foreach (EnvelopeTemplateDefinition etd in templates)
                    {
                        templatesList.Items.Add(new ListItem(etd.Name, etd.TemplateID));

                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log4Net Piece
            log4net.ILog logger = log4net.LogManager.GetLogger(typeof(_Default));
            logger.Info("\n----------------------------------------\n");
            logger.Error(ex.Message);
            logger.Error(ex.StackTrace);
            Response.Write(ex.Message);

        }
        finally
        {
        }

    }


    protected String RandomizeClientUserID()
    {
        Random random = new Random();

        return (random.Next()).ToString();
    }

    protected void createEnvelope()
    {
        FileStream fs = null;

        try
        {
            String userName = ConfigurationManager.AppSettings["API.Email"];
            String password = ConfigurationManager.AppSettings["API.Password"];
            String integratorKey = ConfigurationManager.AppSettings["API.IntegratorKey"];


            String auth = "<DocuSignCredentials><Username>" + userName
                + "</Username><Password>" + password
                + "</Password><IntegratorKey>" + integratorKey
                + "</IntegratorKey></DocuSignCredentials>";
            ServiceReference1.DSAPIServiceSoapClient client = new ServiceReference1.DSAPIServiceSoapClient();

            using (OperationContextScope scope = new System.ServiceModel.OperationContextScope(client.InnerChannel))
            {
                HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers.Add("X-DocuSign-Authentication", auth);
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                CompositeTemplate template = new CompositeTemplate();

                // Set up recipients 
                Recipient[] recipients;
                if (jointEmail.Value.Trim().Equals(""))
                {
                    recipients = new Recipient[1];
                }
                else
                {
                    recipients = new Recipient[2];
                }

                recipients[0] = new Recipient();
                recipients[0].ID = "1";
                recipients[0].Email = email.Value;
                recipients[0].Type = RecipientTypeCode.Signer;
                recipients[0].UserName = firstname.Value + " " + lastname.Value;
                recipients[0].CaptiveInfo = new RecipientCaptiveInfo();

                recipients[0].CaptiveInfo.ClientUserId = RandomizeClientUserID();
                recipients[0].RoutingOrder = 1;
                recipients[0].RoleName = "Signer1";

                // If there is a 2nd recipient, configure 
                if (!jointEmail.Value.Equals(""))
                {
                    recipients[1] = new Recipient();
                    recipients[1].ID = "2";
                    recipients[1].Email = jointEmail.Value;
                    recipients[1].Type = RecipientTypeCode.Signer;
                    recipients[1].UserName = jointFirstname.Value + " " + jointLastname.Value;
                    recipients[1].RoleName = "Signer2";
                    recipients[1].RoutingOrder = 1;
                }

                //Configure the inline templates 
                InlineTemplate inlineTemplate = new InlineTemplate();
                inlineTemplate.Sequence = "2";
                inlineTemplate.Envelope = new Envelope();
                inlineTemplate.Envelope.Recipients = recipients;
                inlineTemplate.Envelope.AccountId = ConfigurationManager.AppSettings["API.AccountId"];

                template.InlineTemplates = new InlineTemplate[] { inlineTemplate };
                // Configure the document
                template.Document = new Document();
                template.Document.ID = "1";
                template.Document.Name = "Sample Document";

                BinaryReader binReader = null;
                String filename = uploadFile.Value;
                if (File.Exists(Server.MapPath("~/App_Data/" + filename)))
                {
                    fs = new FileStream(Server.MapPath("~/App_Data/" + filename), FileMode.Open);
                    binReader = new BinaryReader(fs);
                }
                byte[] PDF = binReader.ReadBytes(System.Convert.ToInt32(fs.Length));
                template.Document.PDFBytes = PDF;
                
                template.Document.TransformPdfFields = true;
                template.Document.FileExtension = "pdf";

                ServerTemplate serverTemplate = new ServerTemplate();

                serverTemplate.Sequence = "1";
                serverTemplate.TemplateID = templatesList.SelectedValue;
                template.ServerTemplates = new ServerTemplate[] {serverTemplate};

                // Set up the envelope
                EnvelopeInformation envInfo = new EnvelopeInformation();
                envInfo.AutoNavigation = true;
                envInfo.AccountId = ConfigurationManager.AppSettings["API.AccountId"];
                envInfo.Subject = "Templates Example";

                //Create envelope with all the composite template information 
                EnvelopeStatus status = client.CreateEnvelopeFromTemplatesAndForms(envInfo, new CompositeTemplate[] { template }, true);
                RequestRecipientTokenAuthenticationAssertion assert = new RequestRecipientTokenAuthenticationAssertion();
                assert.AssertionID = "12345";
                assert.AuthenticationInstant = DateTime.Now;
                assert.AuthenticationMethod = RequestRecipientTokenAuthenticationAssertionAuthenticationMethod.Password;
                assert.SecurityDomain = "www.magicparadigm.com";

                RequestRecipientTokenClientURLs clientURLs = new RequestRecipientTokenClientURLs();

                clientURLs.OnAccessCodeFailed = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnAccessCodeFailed";
                clientURLs.OnCancel = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnCancel";
                clientURLs.OnDecline = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnDecline";
                clientURLs.OnException = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnException";
                clientURLs.OnFaxPending = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnFaxPending";
                clientURLs.OnIdCheckFailed = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnIdCheckFailed";
                clientURLs.OnSessionTimeout = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnSessionTimeout";
                clientURLs.OnTTLExpired = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnTTLExpired";
                clientURLs.OnViewingComplete = ConfigurationManager.AppSettings["RecipientTokenClientURLsPrefix"] + "?envelopeId=" + status.EnvelopeID + "&event=OnViewingComplete";


                String url = Request.Url.AbsoluteUri;

                String recipientToken;

                clientURLs.OnSigningComplete = url.Substring(0, url.LastIndexOf("/")) + "/EmbeddedSigningComplete0.aspx?envelopeID=" + status.EnvelopeID;
                recipientToken = client.RequestRecipientToken(status.EnvelopeID, recipients[0].CaptiveInfo.ClientUserId, recipients[0].UserName, recipients[0].Email, assert, clientURLs);
                Session["envelopeID"] = status.EnvelopeID;
                if (!Request.Browser.Browser.Equals("InternetExplorer") && (!Request.Browser.Browser.Equals("Safari")))
                {
                    docusignFrame.Visible = true;
                    docusignFrame.Src = recipientToken;
                }
                else // Handle IE differently since it does not allow dynamic setting of the iFrame width and height
                {
                    docusignFrameIE.Visible = true;
                    docusignFrameIE.Src = recipientToken;
                }

            }
        }
        catch (Exception ex)
        {
            // Log4Net Piece
            log4net.ILog logger = log4net.LogManager.GetLogger(typeof(_Default));
            logger.Info("\n----------------------------------------\n");
            logger.Error(ex.Message);
            logger.Error(ex.StackTrace);
            Response.Write(ex.Message);

        }
        finally
        {
            if (fs != null)
                fs.Close();
        }
    }
 
}