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

public partial class demos_PDFFormFields : System.Web.UI.Page
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
        signer1MagicPattern.Value = "\\*Signer1_\\*";
        signer2MagicPattern.Value = "\\*Signer2_\\*";
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

                // Assign tabs to a particular recipient in the workflow
                Tab tab1 = new Tab();
                tab1.DocumentID = "1";
                tab1.RecipientID = "1";
                tab1.TabLabel = signer1MagicPattern.Value;
                tab1.Type = TabTypeCode.SignHere;

                Tab tab2 = new Tab();
                tab2.DocumentID = "1";
                tab2.RecipientID = "2";
                tab2.TabLabel = signer2MagicPattern.Value;
                tab2.Type = TabTypeCode.SignHere;

                Tab tab3 = new Tab();
                tab3.DocumentID = "1";
                tab3.RecipientID = "1";
                tab3.TabLabel = signer1MagicPattern.Value;
                tab3.Type = TabTypeCode.DateSigned;

                Tab tab4 = new Tab();
                tab4.DocumentID = "1";
                tab4.RecipientID = "2";
                tab4.TabLabel = signer2MagicPattern.Value;
                tab4.Type = TabTypeCode.DateSigned;

                Tab tab5 = new Tab();
                tab5.DocumentID = "1";
                tab5.RecipientID = "1";
                tab5.TabLabel = signer1MagicPattern.Value;
                tab5.Type = TabTypeCode.Custom;

                Tab tab6 = new Tab();
                tab6.DocumentID = "1";
                tab6.RecipientID = "2";
                tab6.TabLabel = signer2MagicPattern.Value;
                tab6.Type = TabTypeCode.Custom;

                Tab tab7 = new Tab();
                tab7.DocumentID = "1";
                tab7.RecipientID = "1";
                tab7.TabLabel = signer1MagicPattern.Value;
                tab7.Type = TabTypeCode.SignerAttachment;

                Tab tab8 = new Tab();
                tab8.DocumentID = "1";
                tab8.RecipientID = "2";
                tab8.TabLabel = signer2MagicPattern.Value;
                tab8.Type = TabTypeCode.SignerAttachment;

                Tab tab9 = new Tab();
                tab9.DocumentID = "1";
                tab9.RecipientID = "1";
                tab9.TabLabel = signer1MagicPattern.Value;
                tab9.Type = TabTypeCode.Approve;

                Tab tab10 = new Tab();
                tab10.DocumentID = "1";
                tab10.RecipientID = "2";
                tab10.TabLabel = signer2MagicPattern.Value;
                tab10.Type = TabTypeCode.Approve;

                Tab tab11 = new Tab();
                tab11.DocumentID = "1";
                tab11.RecipientID = "1";
                tab11.TabLabel = signer1MagicPattern.Value;
                tab11.Type = TabTypeCode.Decline;

                Tab tab12 = new Tab();
                tab12.DocumentID = "1";
                tab12.RecipientID = "2";
                tab12.TabLabel = signer2MagicPattern.Value;
                tab12.Type = TabTypeCode.Decline;

                Tab tab13 = new Tab();
                tab13.DocumentID = "1";
                tab13.RecipientID = "1";
                tab13.TabLabel = signer1MagicPattern.Value;
                tab13.Type = TabTypeCode.FullName;

                Tab tab14 = new Tab();
                tab14.DocumentID = "1";
                tab14.RecipientID = "2";
                tab14.TabLabel = signer2MagicPattern.Value;
                tab14.Type = TabTypeCode.FullName;

                Tab tab15 = new Tab();
                tab15.DocumentID = "1";
                tab15.RecipientID = "1";
                tab15.TabLabel = signer1MagicPattern.Value;
                tab15.Type = TabTypeCode.FullName;

                Tab tab16 = new Tab();
                tab16.DocumentID = "1";
                tab16.RecipientID = "2";
                tab16.TabLabel = signer2MagicPattern.Value;
                tab16.Type = TabTypeCode.FullName;

                inlineTemplate.Envelope.Tabs = new Tab[] { tab1, tab2, tab3, tab4, tab5, tab6, tab7, tab8, tab9, tab10, tab11, tab12, tab13,tab14, tab15, tab16 };
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

                // Set up the envelope
                EnvelopeInformation envInfo = new EnvelopeInformation();
                envInfo.AutoNavigation = true;
                envInfo.AccountId = ConfigurationManager.AppSettings["API.AccountId"];
                envInfo.Subject = "PDF Form Fields Example";

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

    //protected void SendWithNoTemplate()
    //{
    //    FileStream fs = null;
    //    StreamWriter sw = null;
    //    String filename = "";
    //    try
    //    {

    //        DocuSignAPI.APIServiceSoapClient apiClient = getProxy();
    //        DocuSignAPI.EnvelopeInformation envelopeInfo = new DocuSignAPI.EnvelopeInformation();
    //        envelopeInfo.AccountId = System.Configuration.ConfigurationManager.AppSettings["API.AccountId"];
    //        envelopeInfo.EmailBlurb = "Signing ceremony without any template";
    //        envelopeInfo.Subject = "Create envelope without using templates";

    //        DocuSignAPI.CompositeTemplate template = new DocuSignAPI.CompositeTemplate();

    //        // Define 2 recipients 
    //        DocuSignAPI.Recipient recipient1 = new DocuSignAPI.Recipient();
    //        recipient1.UserName = firstName.Text + " " + lastName.Text;
    //        recipient1.Email = emailAddress.Text;
    //        recipient1.Type = DocuSignAPI.RecipientTypeCode.Signer;
    //        recipient1.RoutingOrder = 1;
    //        recipient1.RoutingOrderSpecified = true;
    //        recipient1.RoleName = "Customer";
    //        recipient1.ID = "1";


    //        DocuSignAPI.Recipient recipient2 = new DocuSignAPI.Recipient();
    //        recipient2.UserName = JointsignerFirstname.Text + " " + JointsignerLastname.Text;
    //        recipient2.Email = JointsignerEmail.Text;
    //        recipient2.Type = DocuSignAPI.RecipientTypeCode.Signer;
    //        recipient2.RoutingOrder = 2;
    //        recipient2.RoutingOrderSpecified = true;
    //        recipient2.RoleName = "JointCustomer";
    //        recipient2.ID = "2";

    //        DocuSignAPI.Recipient[] signers = { recipient1, recipient2 };

    //        //Configure the inline templates 
    //        DocuSignAPI.InlineTemplate inlineTemplate = new DocuSignAPI.InlineTemplate();
    //        inlineTemplate.Sequence = "1";
    //        inlineTemplate.Envelope = new DocuSignAPI.Envelope();
    //        inlineTemplate.Envelope.Recipients = signers;
    //        inlineTemplate.Envelope.AccountId = System.Configuration.ConfigurationManager.AppSettings["API.AccountId"];

    //        DocuSignAPI.Tab firsttab1 = new DocuSignAPI.Tab();
    //        firsttab1.DocumentID = "1";
    //        firsttab1.RecipientID = "1";
    //        firsttab1.TabLabel = "\\*Customer_\\*";

    //        // Associate any fields of type SignHere that have the pattern \*Customer_\* to the recipient with the ID = 1 
    //        firsttab1.Type = DocuSignAPI.TabTypeCode.SignHere;

    //        DocuSignAPI.Tab tab1 = new DocuSignAPI.Tab();
    //        tab1.DocumentID = "1";
    //        tab1.RecipientID = "2";

    //        // Associate any fields of type SignHere that have the pattern \*CustomerJoint_\* to the recipient with the ID = 2 
    //        tab1.TabLabel = "\\*CustomerJoint_\\*";
    //        tab1.Type = DocuSignAPI.TabTypeCode.SignHere;

    //        DocuSignAPI.Tab tab5 = new DocuSignAPI.Tab();
    //        tab5.DocumentID = "1";
    //        tab5.RecipientID = "1";
    //        //               tab5.TabLabel = "\\*Customer_\\*";
    //        tab5.TabLabel = SignerFormRole1.Text;
    //        tab5.Type = DocuSignAPI.TabTypeCode.DateSigned;

    //        DocuSignAPI.Tab tab6 = new DocuSignAPI.Tab();
    //        tab6.DocumentID = "1";
    //        tab6.RecipientID = "1";
    //        //                tab6.TabLabel = "\\*Customer_\\*";
    //        tab6.TabLabel = SignerFormRole1.Text;

    //        tab6.Type = DocuSignAPI.TabTypeCode.Custom;

    //        DocuSignAPI.Tab tab7 = new DocuSignAPI.Tab();
    //        tab7.DocumentID = "1";
    //        tab7.RecipientID = "1";
    //        //                tab7.TabLabel = "\\*Customer_\\*";
    //        tab7.TabLabel = SignerFormRole1.Text;

    //        tab7.Type = DocuSignAPI.TabTypeCode.SignerAttachment;




    //        DocuSignAPI.Tab tab2 = new DocuSignAPI.Tab();
    //        tab2.DocumentID = "1";
    //        tab2.RecipientID = "2";
    //        //                tab2.TabLabel = "\\*CustomerJoint_\\*";
    //        tab2.TabLabel = SignerFormRole2.Text;
    //        tab2.Type = DocuSignAPI.TabTypeCode.DateSigned;

    //        DocuSignAPI.Tab tab3 = new DocuSignAPI.Tab();
    //        tab3.DocumentID = "1";
    //        tab3.RecipientID = "2";
    //        //                tab3.TabLabel = "\\*CustomerJoint_\\*";
    //        tab3.TabLabel = SignerFormRole2.Text;
    //        tab3.Type = DocuSignAPI.TabTypeCode.Custom;

    //        DocuSignAPI.Tab tab4 = new DocuSignAPI.Tab();
    //        tab4.DocumentID = "1";
    //        tab4.RecipientID = "2";
    //        //                tab4.TabLabel = "\\*CustomerJoint_\\*";
    //        tab4.TabLabel = SignerFormRole2.Text;
    //        tab4.Type = DocuSignAPI.TabTypeCode.InitialHere;

    //        inlineTemplate.Envelope.Tabs = new DocuSignAPI.Tab[] { tab1, tab2, tab3, tab4, firsttab1, tab5, tab6, tab7 };
    //        template.InlineTemplates = new DocuSignAPI.InlineTemplate[] { inlineTemplate };
    //        // Configure the document 
    //        template.Document = new DocuSignAPI.Document();
    //        template.Document.ID = "1";
    //        template.Document.Name = "Form Document";
    //        BinaryReader binReader = null;
    //        filename = Label1.Text;
    //        if (File.Exists(Server.MapPath("~/App_Data/" + filename)))
    //        {
    //            fs = new FileStream(Server.MapPath("~/App_Data/" + filename), FileMode.Open);
    //            binReader = new BinaryReader(fs);
    //        }
    //        byte[] PDF = binReader.ReadBytes(System.Convert.ToInt32(fs.Length));

    //        template.Document.PDFBytes = PDF;
    //        template.Document.TransformPdfFields = true;


    //        //Create envelope with all the composite template information 
    //        DocuSignAPI.EnvelopeStatus status = apiClient.CreateEnvelopeFromTemplatesAndForms(envelopeInfo, new DocuSignAPI.CompositeTemplate[] { template }, true);
    //        envelopeID.Text = status.EnvelopeID;
    //    }
    //    catch (Exception ex)
    //    {
    //        if (fs != null)
    //            fs.Close();


    //        fs = new FileStream(Server.MapPath("~/App_Data/" + ConfigurationManager.AppSettings["LogfilePath"]), FileMode.OpenOrCreate);
    //        sw = new StreamWriter(fs);
    //        sw.WriteLine(ex.Message);
    //        Response.Write(ex.Message);
    //        sw.WriteLine(ex.StackTrace);

    //    }
    //    finally
    //    {
    //        if (sw != null)
    //        {
    //            sw.Flush();
    //            sw.Close();
    //        }
    //        if (fs != null) fs.Close();
    //    }
    //}
}