using System;
using System.Collections;
using GeneXus.Utils;
using GeneXus.Resources;
using GeneXus.Application;
using GeneXus.Metadata;
using GeneXus.Cryptography;
using System.Data;
using GeneXus.Data;
using com.genexus;
using GeneXus.Data.ADO;
using GeneXus.Data.NTier;
using GeneXus.Data.NTier.ADO;
using GeneXus.WebControls;
using GeneXus.Http;
using GeneXus.XML;
using GeneXus.Search;
using GeneXus.Encryption;
using GeneXus.Http.Client;
using System.Xml.Serialization;
using System.Runtime.Serialization;
namespace GeneXus.Programs.general {
   public class calculadora : GXDataArea
   {
      public calculadora( )
      {
         context = new GxContext(  );
         DataStoreUtil.LoadDataStores( context);
         dsDefault = context.GetDataStore("Default");
         IsMain = true;
         context.SetDefaultTheme("Calculadora_Bruno", true);
      }

      public calculadora( IGxContext context )
      {
         this.context = context;
         IsMain = false;
         dsDefault = context.GetDataStore("Default");
      }

      public void execute( )
      {
         ExecuteImpl();
      }

      protected override void ExecutePrivate( )
      {
         isStatic = false;
         webExecute();
      }

      protected override void createObjects( )
      {
      }

      protected void INITWEB( )
      {
         initialize_properties( ) ;
         if ( nGotPars == 0 )
         {
            entryPointCalled = false;
            gxfirstwebparm = GetNextPar( );
            gxfirstwebparm_bkp = gxfirstwebparm;
            gxfirstwebparm = DecryptAjaxCall( gxfirstwebparm);
            toggleJsOutput = isJsOutputEnabled( );
            if ( context.isSpaRequest( ) )
            {
               disableJsOutput();
            }
            if ( StringUtil.StrCmp(gxfirstwebparm, "dyncall") == 0 )
            {
               setAjaxCallMode();
               if ( ! IsValidAjaxCall( true) )
               {
                  GxWebError = 1;
                  return  ;
               }
               dyncall( GetNextPar( )) ;
               return  ;
            }
            else if ( StringUtil.StrCmp(gxfirstwebparm, "gxajaxEvt") == 0 )
            {
               setAjaxEventMode();
               if ( ! IsValidAjaxCall( true) )
               {
                  GxWebError = 1;
                  return  ;
               }
               gxfirstwebparm = GetNextPar( );
            }
            else if ( StringUtil.StrCmp(gxfirstwebparm, "gxfullajaxEvt") == 0 )
            {
               if ( ! IsValidAjaxCall( true) )
               {
                  GxWebError = 1;
                  return  ;
               }
               gxfirstwebparm = GetNextPar( );
            }
            else
            {
               if ( ! IsValidAjaxCall( false) )
               {
                  GxWebError = 1;
                  return  ;
               }
               gxfirstwebparm = gxfirstwebparm_bkp;
            }
            if ( toggleJsOutput )
            {
               if ( context.isSpaRequest( ) )
               {
                  enableJsOutput();
               }
            }
         }
         if ( ! context.IsLocalStorageSupported( ) )
         {
            context.PushCurrentUrl();
         }
      }

      public override void webExecute( )
      {
         createObjects();
         initialize();
         INITWEB( ) ;
         if ( ! isAjaxCallMode( ) )
         {
            MasterPageObj = (GXMasterPage) ClassLoader.GetInstance("general.ui.masterunanimosidebar", "GeneXus.Programs.general.ui.masterunanimosidebar", new Object[] {context});
            MasterPageObj.setDataArea(this,false);
            ValidateSpaRequest();
            MasterPageObj.webExecute();
            if ( ( GxWebError == 0 ) && context.isAjaxRequest( ) )
            {
               enableOutput();
               if ( ! context.isAjaxRequest( ) )
               {
                  context.GX_webresponse.AppendHeader("Cache-Control", "no-store");
               }
               if ( ! context.WillRedirect( ) )
               {
                  AddString( context.getJSONResponse( )) ;
               }
               else
               {
                  if ( context.isAjaxRequest( ) )
                  {
                     disableOutput();
                  }
                  RenderHtmlHeaders( ) ;
                  context.Redirect( context.wjLoc );
                  context.DispatchAjaxCommands();
               }
            }
         }
         this.cleanup();
      }

      public override short ExecuteStartEvent( )
      {
         PA032( ) ;
         gxajaxcallmode = (short)((isAjaxCallMode( ) ? 1 : 0));
         if ( ( gxajaxcallmode == 0 ) && ( GxWebError == 0 ) )
         {
            START032( ) ;
         }
         return gxajaxcallmode ;
      }

      public override void RenderHtmlHeaders( )
      {
         GxWebStd.gx_html_headers( context, 0, "", "", Form.Meta, Form.Metaequiv, true);
      }

      public override void RenderHtmlOpenForm( )
      {
         if ( context.isSpaRequest( ) )
         {
            enableOutput();
         }
         context.WriteHtmlText( "<title>") ;
         context.SendWebValue( Form.Caption) ;
         context.WriteHtmlTextNl( "</title>") ;
         if ( context.isSpaRequest( ) )
         {
            disableOutput();
         }
         if ( StringUtil.Len( sDynURL) > 0 )
         {
            context.WriteHtmlText( "<BASE href=\""+sDynURL+"\" />") ;
         }
         define_styles( ) ;
         if ( nGXWrapped != 1 )
         {
            MasterPageObj.master_styles();
         }
         CloseStyles();
         if ( ( ( context.GetBrowserType( ) == 1 ) || ( context.GetBrowserType( ) == 5 ) ) && ( StringUtil.StrCmp(context.GetBrowserVersion( ), "7.0") == 0 ) )
         {
            context.AddJavascriptSource("json2.js", "?"+context.GetBuildNumber( 167520), false, true);
         }
         context.AddJavascriptSource("jquery.js", "?"+context.GetBuildNumber( 167520), false, true);
         context.AddJavascriptSource("gxgral.js", "?"+context.GetBuildNumber( 167520), false, true);
         context.AddJavascriptSource("gxcfg.js", "?"+GetCacheInvalidationToken( ), false, true);
         if ( context.isSpaRequest( ) )
         {
            enableOutput();
         }
         context.WriteHtmlText( Form.Headerrawhtml) ;
         context.CloseHtmlHeader();
         if ( context.isSpaRequest( ) )
         {
            disableOutput();
         }
         FormProcess = " data-HasEnter=\"false\" data-Skiponenter=\"false\"";
         context.WriteHtmlText( "<body ") ;
         if ( StringUtil.StrCmp(context.GetLanguageProperty( "rtl"), "true") == 0 )
         {
            context.WriteHtmlText( " dir=\"rtl\" ") ;
         }
         bodyStyle = "" + "background-color:" + context.BuildHTMLColor( Form.Backcolor) + ";color:" + context.BuildHTMLColor( Form.Textcolor) + ";";
         if ( nGXWrapped == 0 )
         {
            bodyStyle += "-moz-opacity:0;opacity:0;";
         }
         if ( ! ( String.IsNullOrEmpty(StringUtil.RTrim( Form.Background)) ) )
         {
            bodyStyle += " background-image:url(" + context.convertURL( Form.Background) + ")";
         }
         context.WriteHtmlText( " "+"class=\"form-horizontal Form\""+" "+ "style='"+bodyStyle+"'") ;
         context.WriteHtmlText( FormProcess+">") ;
         context.skipLines(1);
         context.WriteHtmlTextNl( "<form id=\"MAINFORM\" autocomplete=\"off\" name=\"MAINFORM\" method=\"post\" tabindex=-1  class=\"form-horizontal Form\" data-gx-class=\"form-horizontal Form\" novalidate action=\""+formatLink("general.calculadora.aspx") +"\">") ;
         GxWebStd.gx_hidden_field( context, "_EventName", "");
         GxWebStd.gx_hidden_field( context, "_EventGridId", "");
         GxWebStd.gx_hidden_field( context, "_EventRowId", "");
         context.WriteHtmlText( "<div style=\"height:0;overflow:hidden\"><input type=\"submit\" title=\"submit\"  disabled></div>") ;
         AssignProp("", false, "FORM", "Class", "form-horizontal Form", true);
         toggleJsOutput = isJsOutputEnabled( );
         if ( context.isSpaRequest( ) )
         {
            disableJsOutput();
         }
      }

      protected void send_integrity_footer_hashes( )
      {
         GXKey = Decrypt64( context.GetCookie( "GX_SESSION_ID"), Crypto.GetServerKey( ));
      }

      protected void SendCloseFormHiddens( )
      {
         /* Send hidden variables. */
         /* Send saved values. */
         send_integrity_footer_hashes( ) ;
         GxWebStd.gx_hidden_field( context, "vRESULTADO", AV5Resultado);
         GxWebStd.gx_hidden_field( context, "vAUXRESULTADO", AV7auxResultado);
         GxWebStd.gx_hidden_field( context, "vOPERACAO", AV6Operacao);
         GxWebStd.gx_boolean_hidden_field( context, "vCALCULOSUCESSO", AV12CalculoSucesso);
         GxWebStd.gx_hidden_field( context, "vNUMERO1", StringUtil.LTrim( StringUtil.NToC( AV11numero1, 18, 10, ".", "")));
         GxWebStd.gx_hidden_field( context, "vNUMERO2", StringUtil.LTrim( StringUtil.NToC( AV10numero2, 18, 10, ".", "")));
      }

      public override void RenderHtmlCloseForm( )
      {
         SendCloseFormHiddens( ) ;
         GxWebStd.gx_hidden_field( context, "GX_FocusControl", GX_FocusControl);
         SendAjaxEncryptionKey();
         SendSecurityToken((string)(sPrefix));
         SendComponentObjects();
         SendServerCommands();
         SendState();
         if ( context.isSpaRequest( ) )
         {
            disableOutput();
         }
         context.WriteHtmlTextNl( "</form>") ;
         if ( context.isSpaRequest( ) )
         {
            enableOutput();
         }
         include_jscripts( ) ;
      }

      public override void RenderHtmlContent( )
      {
         gxajaxcallmode = (short)((isAjaxCallMode( ) ? 1 : 0));
         if ( ( gxajaxcallmode == 0 ) && ( GxWebError == 0 ) )
         {
            context.WriteHtmlText( "<div") ;
            GxWebStd.ClassAttribute( context, "gx-ct-body"+" "+(String.IsNullOrEmpty(StringUtil.RTrim( Form.Class)) ? "form-horizontal Form" : Form.Class)+"-fx");
            context.WriteHtmlText( ">") ;
            WE032( ) ;
            context.WriteHtmlText( "</div>") ;
         }
      }

      public override void DispatchEvents( )
      {
         EVT032( ) ;
      }

      public override bool HasEnterEvent( )
      {
         return false ;
      }

      public override GXWebForm GetForm( )
      {
         return Form ;
      }

      public override string GetSelfLink( )
      {
         return formatLink("general.calculadora.aspx")  ;
      }

      public override string GetPgmname( )
      {
         return "General.Calculadora" ;
      }

      public override string GetPgmdesc( )
      {
         return "Calculadora" ;
      }

      protected void WB030( )
      {
         if ( context.isAjaxRequest( ) )
         {
            disableOutput();
         }
         if ( ! wbLoad )
         {
            if ( nGXWrapped == 1 )
            {
               RenderHtmlHeaders( ) ;
               RenderHtmlOpenForm( ) ;
            }
            GxWebStd.gx_msg_list( context, "", context.GX_msglist.DisplayMode, "", "", "", "false");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "Section", "start", "top", " "+"data-gx-base-lib=\"none\""+" "+"data-abstract-form"+" ", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, divMaintable_Internalname, 1, 0, "px", 0, "px", "painel", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12", "start", "top", "", "", "div");
            /* Text block */
            GxWebStd.gx_label_ctrl( context, lblTextblock1_Internalname, "CALCULADORA TOPSTER", "", "", lblTextblock1_Jsonclick, "'"+""+"'"+",false,"+"'"+""+"'", "", "Title", 0, "", 1, 1, 0, 0, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "form-group gx-form-group", "start", "top", ""+" data-gx-for=\""+edtavHistorico_Internalname+"\"", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-sm-9 gx-attribute", "start", "top", "", "", "div");
            /* Single line edit */
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 11,'',false,'',0)\"";
            GxWebStd.gx_single_line_edit( context, edtavHistorico_Internalname, AV27historico, StringUtil.RTrim( context.localUtil.Format( AV27historico, "")), TempTags+" onchange=\""+""+";gx.evt.onchange(this, event)\" "+" onblur=\""+""+";gx.evt.onblur(this,11);\"", "'"+""+"'"+",false,"+"'"+""+"'", "", "", "", "", edtavHistorico_Jsonclick, 0, "Attribute historico", "", "", "", "", 1, edtavHistorico_Enabled, 0, "text", "", 40, "chr", 1, "row", 40, 0, 0, 0, 0, -1, -1, true, "", "start", true, "", "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "form-group gx-form-group", "start", "top", ""+" data-gx-for=\""+edtavMostraruser_Internalname+"\"", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-sm-9 gx-attribute", "start", "top", "", "", "div");
            /* Single line edit */
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 16,'',false,'',0)\"";
            GxWebStd.gx_single_line_edit( context, edtavMostraruser_Internalname, AV13MostrarUser, StringUtil.RTrim( context.localUtil.Format( AV13MostrarUser, "")), TempTags+" onchange=\""+""+";gx.evt.onchange(this, event)\" "+" onblur=\""+""+";gx.evt.onblur(this,16);\"", "'"+""+"'"+",false,"+"'"+""+"'", "", "", "", "", edtavMostraruser_Jsonclick, 0, "Attribute", "", "", "", "", 1, edtavMostraruser_Enabled, 0, "text", "", 40, "chr", 1, "row", 40, 0, 0, 0, 0, -1, -1, true, "", "start", true, "", "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-6", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 19,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttCancelartudo_Internalname, "", "Cancelar Tudo", bttCancelartudo_Jsonclick, 7, "Cancelar Tudo", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e11031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-6", "start", "top", "", "", "div");
            wb_table1_21_032( true) ;
         }
         else
         {
            wb_table1_21_032( false) ;
         }
         return  ;
      }

      protected void wb_table1_21_032e( bool wbgen )
      {
         if ( wbgen )
         {
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-4", "start", "top", "", "", "div");
            wb_table2_27_032( true) ;
         }
         else
         {
            wb_table2_27_032( false) ;
         }
         return  ;
      }

      protected void wb_table2_27_032e( bool wbgen )
      {
         if ( wbgen )
         {
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-4", "start", "top", "", "", "div");
            wb_table3_32_032( true) ;
         }
         else
         {
            wb_table3_32_032( false) ;
         }
         return  ;
      }

      protected void wb_table3_32_032e( bool wbgen )
      {
         if ( wbgen )
         {
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-4", "end", "top", "", "", "div");
            wb_table4_37_032( true) ;
         }
         else
         {
            wb_table4_37_032( false) ;
         }
         return  ;
      }

      protected void wb_table4_37_032e( bool wbgen )
      {
         if ( wbgen )
         {
            GxWebStd.gx_div_end( context, "end", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 43,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd7_Internalname, "", "7", bttAdd7_Jsonclick, 7, "7", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e12031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 45,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd8_Internalname, "", "8", bttAdd8_Jsonclick, 7, "8", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e13031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 47,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd9_Internalname, "", "9", bttAdd9_Jsonclick, 7, "9", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e14031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 49,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttDividir_Internalname, "", "/", bttDividir_Jsonclick, 5, "/", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'DIVIDIR\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 52,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd4_Internalname, "", "4", bttAdd4_Jsonclick, 7, "4", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e15031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 54,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd5_Internalname, "", "5", bttAdd5_Jsonclick, 7, "5", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e16031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 56,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd6_Internalname, "", "6", bttAdd6_Jsonclick, 7, "6", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e17031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 58,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttMultiplicar_Internalname, "", "X", bttMultiplicar_Jsonclick, 5, "X", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'MULTIPLICAR\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 61,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd1_Internalname, "", "1", bttAdd1_Jsonclick, 7, "1", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e18031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 63,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd2_Internalname, "", "2", bttAdd2_Jsonclick, 7, "2", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e19031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 65,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd3_Internalname, "", "3", bttAdd3_Jsonclick, 7, "3", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e20031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 67,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttSubtrair_Internalname, "", "-", bttSubtrair_Jsonclick, 5, "-", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'SUBTRAIR\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 70,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttInvertersinal_Internalname, "", "+/-", bttInvertersinal_Jsonclick, 5, "+/-", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'TROCARSINAL\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 72,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdd0_Internalname, "", "0", bttAdd0_Jsonclick, 7, "0", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e21031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 74,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAddvirgula_Internalname, "", ",", bttAddvirgula_Jsonclick, 7, ",", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e22031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12 col-sm-3", "start", "top", "", "", "div");
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 76,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAdicionar_Internalname, "", "+", bttAdicionar_Jsonclick, 5, "+", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'ADICIONAR\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "row", "start", "top", "", "", "div");
            /* Div Control */
            GxWebStd.gx_div_start( context, "", 1, 0, "px", 0, "px", "col-xs-12", "end", "top", "", "", "div");
            wb_table5_79_032( true) ;
         }
         else
         {
            wb_table5_79_032( false) ;
         }
         return  ;
      }

      protected void wb_table5_79_032e( bool wbgen )
      {
         if ( wbgen )
         {
            GxWebStd.gx_div_end( context, "end", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
            GxWebStd.gx_div_end( context, "start", "top", "div");
         }
         wbLoad = true;
      }

      protected void START032( )
      {
         wbLoad = false;
         wbEnd = 0;
         wbStart = 0;
         if ( ! context.isSpaRequest( ) )
         {
            if ( context.ExposeMetadata( ) )
            {
               Form.Meta.addItem("generator", "GeneXus .NET 18_0_7-179127", 0) ;
            }
         }
         Form.Meta.addItem("description", "Calculadora", 0) ;
         context.wjLoc = "";
         context.nUserReturn = 0;
         context.wbHandled = 0;
         if ( StringUtil.StrCmp(context.GetRequestMethod( ), "POST") == 0 )
         {
         }
         wbErr = false;
         STRUP030( ) ;
      }

      protected void WS032( )
      {
         START032( ) ;
         EVT032( ) ;
      }

      protected void EVT032( )
      {
         if ( StringUtil.StrCmp(context.GetRequestMethod( ), "POST") == 0 )
         {
            if ( ! context.WillRedirect( ) && ( context.nUserReturn != 1 ) && ! wbErr )
            {
               /* Read Web Panel buttons. */
               sEvt = cgiGet( "_EventName");
               EvtGridId = cgiGet( "_EventGridId");
               EvtRowId = cgiGet( "_EventRowId");
               if ( StringUtil.Len( sEvt) > 0 )
               {
                  sEvtType = StringUtil.Left( sEvt, 1);
                  sEvt = StringUtil.Right( sEvt, (short)(StringUtil.Len( sEvt)-1));
                  if ( StringUtil.StrCmp(sEvtType, "M") != 0 )
                  {
                     if ( StringUtil.StrCmp(sEvtType, "E") == 0 )
                     {
                        sEvtType = StringUtil.Right( sEvt, 1);
                        if ( StringUtil.StrCmp(sEvtType, ".") == 0 )
                        {
                           sEvt = StringUtil.Left( sEvt, (short)(StringUtil.Len( sEvt)-1));
                           if ( StringUtil.StrCmp(sEvt, "RFR") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'DIVIDIR'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'Dividir' */
                              E23032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'MULTIPLICAR'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'Multiplicar' */
                              E24032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'SUBTRAIR'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'Subtrair' */
                              E25032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'ADICIONAR'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'Adicionar' */
                              E26032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'AOQUADRADO'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'AoQuadrado' */
                              E27032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'RAIZQUADRADA'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'RaizQuadrada' */
                              E28032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'TROCARSINAL'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'TrocarSinal' */
                              E29032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'CHAMARRESULTADO'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'ChamarResultado' */
                              E30032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "'POTENCIA'") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: 'Potencia' */
                              E31032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "LOAD") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              /* Execute user event: Load */
                              E32032 ();
                           }
                           else if ( StringUtil.StrCmp(sEvt, "ENTER") == 0 )
                           {
                              context.wbHandled = 1;
                              if ( ! wbErr )
                              {
                                 Rfr0gs = false;
                                 if ( ! Rfr0gs )
                                 {
                                 }
                                 dynload_actions( ) ;
                              }
                              /* No code required for Cancel button. It is implemented as the Reset button. */
                           }
                           else if ( StringUtil.StrCmp(sEvt, "LSCR") == 0 )
                           {
                              context.wbHandled = 1;
                              dynload_actions( ) ;
                              dynload_actions( ) ;
                           }
                        }
                        else
                        {
                        }
                     }
                     context.wbHandled = 1;
                  }
               }
            }
         }
      }

      protected void WE032( )
      {
         if ( ! GxWebStd.gx_redirect( context) )
         {
            Rfr0gs = true;
            Refresh( ) ;
            if ( ! GxWebStd.gx_redirect( context) )
            {
               if ( nGXWrapped == 1 )
               {
                  RenderHtmlCloseForm( ) ;
               }
            }
         }
      }

      protected void PA032( )
      {
         if ( nDonePA == 0 )
         {
            if ( String.IsNullOrEmpty(StringUtil.RTrim( context.GetCookie( "GX_SESSION_ID"))) )
            {
               gxcookieaux = context.SetCookie( "GX_SESSION_ID", Encrypt64( Crypto.GetEncryptionKey( ), Crypto.GetServerKey( )), "", (DateTime)(DateTime.MinValue), "", (short)(context.GetHttpSecure( )));
            }
            GXKey = Decrypt64( context.GetCookie( "GX_SESSION_ID"), Crypto.GetServerKey( ));
            toggleJsOutput = isJsOutputEnabled( );
            if ( context.isSpaRequest( ) )
            {
               disableJsOutput();
            }
            init_web_controls( ) ;
            if ( toggleJsOutput )
            {
               if ( context.isSpaRequest( ) )
               {
                  enableJsOutput();
               }
            }
            if ( ! context.isAjaxRequest( ) )
            {
               GX_FocusControl = edtavHistorico_Internalname;
               AssignAttri("", false, "GX_FocusControl", GX_FocusControl);
            }
            nDonePA = 1;
         }
      }

      protected void dynload_actions( )
      {
         /* End function dynload_actions */
      }

      protected void send_integrity_hashes( )
      {
      }

      protected void clear_multi_value_controls( )
      {
         if ( context.isAjaxRequest( ) )
         {
            dynload_actions( ) ;
            before_start_formulas( ) ;
         }
      }

      protected void fix_multi_value_controls( )
      {
      }

      public void Refresh( )
      {
         send_integrity_hashes( ) ;
         RF032( ) ;
         if ( isFullAjaxMode( ) )
         {
            send_integrity_footer_hashes( ) ;
         }
      }

      protected void initialize_formulas( )
      {
         /* GeneXus formulas. */
         edtavHistorico_Enabled = 0;
         AssignProp("", false, edtavHistorico_Internalname, "Enabled", StringUtil.LTrimStr( (decimal)(edtavHistorico_Enabled), 5, 0), true);
         edtavMostraruser_Enabled = 0;
         AssignProp("", false, edtavMostraruser_Internalname, "Enabled", StringUtil.LTrimStr( (decimal)(edtavMostraruser_Enabled), 5, 0), true);
      }

      protected void RF032( )
      {
         initialize_formulas( ) ;
         clear_multi_value_controls( ) ;
         gxdyncontrolsrefreshing = true;
         fix_multi_value_controls( ) ;
         gxdyncontrolsrefreshing = false;
         if ( ! context.WillRedirect( ) && ( context.nUserReturn != 1 ) )
         {
            /* Execute user event: Load */
            E32032 ();
            WB030( ) ;
         }
      }

      protected void send_integrity_lvl_hashes032( )
      {
      }

      protected void before_start_formulas( )
      {
         edtavHistorico_Enabled = 0;
         AssignProp("", false, edtavHistorico_Internalname, "Enabled", StringUtil.LTrimStr( (decimal)(edtavHistorico_Enabled), 5, 0), true);
         edtavMostraruser_Enabled = 0;
         AssignProp("", false, edtavMostraruser_Internalname, "Enabled", StringUtil.LTrimStr( (decimal)(edtavMostraruser_Enabled), 5, 0), true);
         fix_multi_value_controls( ) ;
      }

      protected void STRUP030( )
      {
         /* Before Start, stand alone formulas. */
         before_start_formulas( ) ;
         context.wbGlbDoneStart = 1;
         /* After Start, stand alone formulas. */
         if ( StringUtil.StrCmp(context.GetRequestMethod( ), "POST") == 0 )
         {
            /* Read saved SDTs. */
            /* Read saved values. */
            AV12CalculoSucesso = StringUtil.StrToBool( cgiGet( "vCALCULOSUCESSO"));
            AV6Operacao = cgiGet( "vOPERACAO");
            AV7auxResultado = cgiGet( "vAUXRESULTADO");
            AV5Resultado = cgiGet( "vRESULTADO");
            AV11numero1 = context.localUtil.CToN( cgiGet( "vNUMERO1"), ".", ",");
            AV10numero2 = context.localUtil.CToN( cgiGet( "vNUMERO2"), ".", ",");
            /* Read variables values. */
            AV27historico = cgiGet( edtavHistorico_Internalname);
            AssignAttri("", false, "AV27historico", AV27historico);
            AV13MostrarUser = cgiGet( edtavMostraruser_Internalname);
            AssignAttri("", false, "AV13MostrarUser", AV13MostrarUser);
            /* Read subfile selected row values. */
            /* Read hidden variables. */
            GXKey = Decrypt64( context.GetCookie( "GX_SESSION_ID"), Crypto.GetServerKey( ));
         }
         else
         {
            dynload_actions( ) ;
         }
      }

      protected void S112( )
      {
         /* 'BOTAORESULTADO' Routine */
         returnInSub = false;
         AV10numero2 = NumberUtil.Val( AV5Resultado, ".");
         AV11numero1 = NumberUtil.Val( AV7auxResultado, ".");
         if ( StringUtil.StrCmp(AV6Operacao, "/") == 0 )
         {
            if ( ! ( AV10numero2 == Convert.ToDecimal( 0 )) )
            {
               if ( ( AV11numero1 == Convert.ToDecimal( 0 )) )
               {
                  AV5Resultado = "0";
                  AssignAttri("", false, "AV5Resultado", AV5Resultado);
               }
               else
               {
                  AV11numero1 = (decimal)((AV11numero1/ (decimal)(AV10numero2)));
                  AV5Resultado = context.localUtil.Format( AV11numero1, "ZZZZZZ9.9999999999");
                  AssignAttri("", false, "AV5Resultado", AV5Resultado);
               }
            }
            else
            {
               GX_msglist.addItem("Não é possivel realizar o cálculo");
               /* Execute user subroutine: 'CANCELARTUDO' */
               S142 ();
               if (returnInSub) return;
            }
         }
         else if ( StringUtil.StrCmp(AV6Operacao, "*") == 0 )
         {
            AV11numero1 = (decimal)((AV11numero1*AV10numero2));
            AV5Resultado = context.localUtil.Format( AV11numero1, "ZZZZZZ9.9999999999");
            AssignAttri("", false, "AV5Resultado", AV5Resultado);
         }
         else if ( StringUtil.StrCmp(AV6Operacao, "-") == 0 )
         {
            AV11numero1 = (decimal)((AV11numero1-AV10numero2));
            AV5Resultado = context.localUtil.Format( AV11numero1, "ZZZZZZ9.9999999999");
            AssignAttri("", false, "AV5Resultado", AV5Resultado);
         }
         else if ( StringUtil.StrCmp(AV6Operacao, "+") == 0 )
         {
            AV11numero1 = (decimal)((AV11numero1+AV10numero2));
            AV5Resultado = context.localUtil.Format( AV11numero1, "ZZZZZZ9.9999999999");
            AssignAttri("", false, "AV5Resultado", AV5Resultado);
         }
         else if ( StringUtil.StrCmp(AV6Operacao, "²") == 0 )
         {
            AV7auxResultado = AV5Resultado;
            AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
            AV11numero1 = (decimal)((AV10numero2*AV10numero2));
            AV5Resultado = context.localUtil.Format( AV11numero1, "ZZZZZZ9.9999999999");
            AssignAttri("", false, "AV5Resultado", AV5Resultado);
         }
         else if ( StringUtil.StrCmp(AV6Operacao, "?") == 0 )
         {
            AV7auxResultado = AV5Resultado;
            AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
            if ( ( NumberUtil.Val( AV5Resultado, ".") < Convert.ToDecimal( 0 )) )
            {
               AV5Resultado = "Entrada Inválida";
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
            else
            {
               AV18expressionDataTypeVariable.Expression = "sqrt( a )";
               AV18expressionDataTypeVariable.Variables.Set("a", AV5Resultado);
               AV11numero1 = (decimal)(AV18expressionDataTypeVariable.Evaluate());
               AV5Resultado = StringUtil.Str( AV11numero1, 18, 10);
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
         }
         else if ( StringUtil.StrCmp(AV6Operacao, "^") == 0 )
         {
            if ( ( NumberUtil.Val( AV5Resultado, ".") > Convert.ToDecimal( 0 )) )
            {
               AV15Count = 1;
               while ( (Convert.ToDecimal( AV15Count ) <= NumberUtil.Val( AV5Resultado, ".") - 1 ) )
               {
                  AV11numero1 = (decimal)(AV11numero1*(NumberUtil.Val( AV7auxResultado, ".")));
                  AV15Count = (short)(AV15Count+1);
               }
               if ( ( NumberUtil.Val( AV5Resultado, ".") == Convert.ToDecimal( 1 )) )
               {
                  AV11numero1 = NumberUtil.Val( AV7auxResultado, ".");
               }
            }
            else
            {
               if ( ( NumberUtil.Val( AV5Resultado, ".") == Convert.ToDecimal( 0 )) )
               {
                  AV11numero1 = (decimal)(1);
               }
            }
            AV5Resultado = context.localUtil.Format( AV11numero1, "ZZZZZZ9.9999999999");
            AssignAttri("", false, "AV5Resultado", AV5Resultado);
         }
         /* Execute user subroutine: 'HISTORICOCALCULO' */
         S152 ();
         if (returnInSub) return;
         AV7auxResultado = "";
         AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
         AV12CalculoSucesso = true;
         AssignAttri("", false, "AV12CalculoSucesso", AV12CalculoSucesso);
         /* Execute user subroutine: 'FORMATARNUMERO' */
         S132 ();
         if (returnInSub) return;
         AV13MostrarUser = AV5Resultado;
         AssignAttri("", false, "AV13MostrarUser", AV13MostrarUser);
         AV6Operacao = "";
         AssignAttri("", false, "AV6Operacao", AV6Operacao);
      }

      protected void E23032( )
      {
         /* 'Dividir' Routine */
         returnInSub = false;
         AV12CalculoSucesso = false;
         AssignAttri("", false, "AV12CalculoSucesso", AV12CalculoSucesso);
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV5Resultado)) || ! String.IsNullOrEmpty(StringUtil.RTrim( AV7auxResultado)) )
         {
            if ( String.IsNullOrEmpty(StringUtil.RTrim( AV6Operacao)) )
            {
               AV7auxResultado = AV5Resultado;
               AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
               AV5Resultado = "";
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
            if ( StringUtil.StrCmp(AV6Operacao, "/") == 0 )
            {
               /* Execute user subroutine: 'BOTAORESULTADO' */
               S112 ();
               if (returnInSub) return;
            }
            else
            {
               AV6Operacao = "/";
               AssignAttri("", false, "AV6Operacao", AV6Operacao);
            }
            /* Execute user subroutine: 'MOSTRARUSUARIO' */
            S122 ();
            if (returnInSub) return;
         }
         else
         {
            GX_msglist.addItem("Por favor, insira o primeiro número");
         }
         /*  Sending Event outputs  */
      }

      protected void E24032( )
      {
         /* 'Multiplicar' Routine */
         returnInSub = false;
         AV12CalculoSucesso = false;
         AssignAttri("", false, "AV12CalculoSucesso", AV12CalculoSucesso);
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV5Resultado)) || ! String.IsNullOrEmpty(StringUtil.RTrim( AV7auxResultado)) )
         {
            if ( String.IsNullOrEmpty(StringUtil.RTrim( AV6Operacao)) )
            {
               AV7auxResultado = AV5Resultado;
               AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
               AV5Resultado = "";
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
            if ( StringUtil.StrCmp(AV6Operacao, "*") == 0 )
            {
               /* Execute user subroutine: 'BOTAORESULTADO' */
               S112 ();
               if (returnInSub) return;
            }
            else
            {
               AV6Operacao = "*";
               AssignAttri("", false, "AV6Operacao", AV6Operacao);
            }
            /* Execute user subroutine: 'MOSTRARUSUARIO' */
            S122 ();
            if (returnInSub) return;
         }
         else
         {
            GX_msglist.addItem("Por favor, insira o primeiro número");
         }
         /*  Sending Event outputs  */
      }

      protected void E25032( )
      {
         /* 'Subtrair' Routine */
         returnInSub = false;
         AV12CalculoSucesso = false;
         AssignAttri("", false, "AV12CalculoSucesso", AV12CalculoSucesso);
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV5Resultado)) || ! String.IsNullOrEmpty(StringUtil.RTrim( AV7auxResultado)) )
         {
            if ( String.IsNullOrEmpty(StringUtil.RTrim( AV6Operacao)) )
            {
               AV7auxResultado = AV5Resultado;
               AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
               AV5Resultado = "";
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
            if ( StringUtil.StrCmp(AV6Operacao, "-") == 0 )
            {
               /* Execute user subroutine: 'BOTAORESULTADO' */
               S112 ();
               if (returnInSub) return;
            }
            else
            {
               AV6Operacao = "-";
               AssignAttri("", false, "AV6Operacao", AV6Operacao);
            }
            /* Execute user subroutine: 'MOSTRARUSUARIO' */
            S122 ();
            if (returnInSub) return;
         }
         else
         {
            GX_msglist.addItem("Por favor, insira o primeiro número");
         }
         /*  Sending Event outputs  */
      }

      protected void E26032( )
      {
         /* 'Adicionar' Routine */
         returnInSub = false;
         AV12CalculoSucesso = false;
         AssignAttri("", false, "AV12CalculoSucesso", AV12CalculoSucesso);
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV5Resultado)) || ! String.IsNullOrEmpty(StringUtil.RTrim( AV7auxResultado)) )
         {
            if ( String.IsNullOrEmpty(StringUtil.RTrim( AV6Operacao)) )
            {
               AV7auxResultado = AV5Resultado;
               AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
               AV5Resultado = "";
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
            if ( StringUtil.StrCmp(AV6Operacao, "+") == 0 )
            {
               /* Execute user subroutine: 'BOTAORESULTADO' */
               S112 ();
               if (returnInSub) return;
            }
            else
            {
               AV6Operacao = "+";
               AssignAttri("", false, "AV6Operacao", AV6Operacao);
            }
            /* Execute user subroutine: 'MOSTRARUSUARIO' */
            S122 ();
            if (returnInSub) return;
         }
         else
         {
            GX_msglist.addItem("Por favor, insira o primeiro número");
         }
         /*  Sending Event outputs  */
      }

      protected void E27032( )
      {
         /* 'AoQuadrado' Routine */
         returnInSub = false;
         AV12CalculoSucesso = false;
         AssignAttri("", false, "AV12CalculoSucesso", AV12CalculoSucesso);
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV5Resultado)) )
         {
            AV6Operacao = "²";
            AssignAttri("", false, "AV6Operacao", AV6Operacao);
            /* Execute user subroutine: 'BOTAORESULTADO' */
            S112 ();
            if (returnInSub) return;
         }
         else
         {
            GX_msglist.addItem("Por favor, insira o primeiro número");
         }
         /*  Sending Event outputs  */
      }

      protected void E28032( )
      {
         /* 'RaizQuadrada' Routine */
         returnInSub = false;
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV6Operacao)) )
         {
            AV5Resultado = AV7auxResultado;
            AssignAttri("", false, "AV5Resultado", AV5Resultado);
         }
         AV12CalculoSucesso = false;
         AssignAttri("", false, "AV12CalculoSucesso", AV12CalculoSucesso);
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV5Resultado)) )
         {
            AV6Operacao = "?";
            AssignAttri("", false, "AV6Operacao", AV6Operacao);
            /* Execute user subroutine: 'BOTAORESULTADO' */
            S112 ();
            if (returnInSub) return;
         }
         else
         {
            GX_msglist.addItem("Por favor, insira o primeiro número");
         }
         /*  Sending Event outputs  */
      }

      protected void E29032( )
      {
         /* 'TrocarSinal' Routine */
         returnInSub = false;
         if ( ( AV12CalculoSucesso ) && ! String.IsNullOrEmpty(StringUtil.RTrim( AV7auxResultado)) )
         {
            AV11numero1 = (decimal)(NumberUtil.Val( AV7auxResultado, ".")*-1);
            AV7auxResultado = context.localUtil.Format( AV11numero1, "ZZZZZZ9.9999999999");
            AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
         }
         else
         {
            if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV5Resultado)) )
            {
               AV10numero2 = (decimal)(NumberUtil.Val( AV5Resultado, ".")*-1);
               AV5Resultado = context.localUtil.Format( AV10numero2, "ZZZZZZ9.9999999999");
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
            else
            {
               GX_msglist.addItem("Por Favor Informe um número");
            }
         }
         /* Execute user subroutine: 'FORMATARNUMERO' */
         S132 ();
         if (returnInSub) return;
         /* Execute user subroutine: 'MOSTRARUSUARIO' */
         S122 ();
         if (returnInSub) return;
         /*  Sending Event outputs  */
      }

      protected void E30032( )
      {
         /* 'ChamarResultado' Routine */
         returnInSub = false;
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV7auxResultado)) || ! String.IsNullOrEmpty(StringUtil.RTrim( AV7auxResultado)) )
         {
            /* Execute user subroutine: 'BOTAORESULTADO' */
            S112 ();
            if (returnInSub) return;
         }
         else
         {
            GX_msglist.addItem("Informe o número e o cálculo desejado");
         }
         /*  Sending Event outputs  */
      }

      protected void S152( )
      {
         /* 'HISTORICOCALCULO' Routine */
         returnInSub = false;
         if ( ( StringUtil.StrCmp(AV6Operacao, "?") == 0 ) || ( StringUtil.StrCmp(AV6Operacao, "²") == 0 ) )
         {
            /* Execute user subroutine: 'FORMATARNUMERO' */
            S132 ();
            if (returnInSub) return;
            /* Execute user subroutine: 'MOSTRARUSUARIO' */
            S122 ();
            if (returnInSub) return;
         }
         AV27historico = AV13MostrarUser;
         AssignAttri("", false, "AV27historico", AV27historico);
      }

      protected void S142( )
      {
         /* 'CANCELARTUDO' Routine */
         returnInSub = false;
         AV5Resultado = "";
         AssignAttri("", false, "AV5Resultado", AV5Resultado);
         AV7auxResultado = "";
         AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
         AV11numero1 = 0;
         AV10numero2 = 0;
         AV6Operacao = "";
         AssignAttri("", false, "AV6Operacao", AV6Operacao);
         AV27historico = "";
         AssignAttri("", false, "AV27historico", AV27historico);
         /* Execute user subroutine: 'MOSTRARUSUARIO' */
         S122 ();
         if (returnInSub) return;
      }

      protected void S122( )
      {
         /* 'MOSTRARUSUARIO' Routine */
         returnInSub = false;
         if ( AV12CalculoSucesso )
         {
            AV13MostrarUser = AV5Resultado + " " + AV6Operacao + " " + AV7auxResultado;
            AssignAttri("", false, "AV13MostrarUser", AV13MostrarUser);
         }
         else
         {
            AV13MostrarUser = AV7auxResultado + " " + AV6Operacao + " " + AV5Resultado;
            AssignAttri("", false, "AV13MostrarUser", AV13MostrarUser);
            if ( StringUtil.StrCmp(AV6Operacao, "²") == 0 )
            {
               AV13MostrarUser = "sqr(" + AV7auxResultado + ")";
               AssignAttri("", false, "AV13MostrarUser", AV13MostrarUser);
            }
            else
            {
               if ( StringUtil.StrCmp(AV6Operacao, "?") == 0 )
               {
                  AV13MostrarUser = "?(" + AV7auxResultado + ")";
                  AssignAttri("", false, "AV13MostrarUser", AV13MostrarUser);
               }
            }
         }
      }

      protected void S132( )
      {
         /* 'FORMATARNUMERO' Routine */
         returnInSub = false;
         while ( StringUtil.EndsWith( AV7auxResultado, "0") )
         {
            if ( StringUtil.Contains( AV7auxResultado, ".") )
            {
               AV7auxResultado = StringUtil.PadL( AV7auxResultado, (short)(StringUtil.Len( AV7auxResultado)-1), " ");
               AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
            }
         }
         while ( StringUtil.EndsWith( AV5Resultado, "0") )
         {
            if ( StringUtil.Contains( AV5Resultado, ".") )
            {
               AV5Resultado = StringUtil.PadL( AV5Resultado, (short)(StringUtil.Len( AV5Resultado)-1), " ");
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
         }
         if ( StringUtil.EndsWith( AV7auxResultado, ".") )
         {
            AV7auxResultado = StringUtil.StringReplace( AV7auxResultado, ".", "");
            AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
         }
         if ( StringUtil.EndsWith( AV5Resultado, ".") )
         {
            AV5Resultado = StringUtil.StringReplace( AV5Resultado, ".", "");
            AssignAttri("", false, "AV5Resultado", AV5Resultado);
         }
      }

      protected void E31032( )
      {
         /* 'Potencia' Routine */
         returnInSub = false;
         AV12CalculoSucesso = false;
         AssignAttri("", false, "AV12CalculoSucesso", AV12CalculoSucesso);
         if ( ! String.IsNullOrEmpty(StringUtil.RTrim( AV5Resultado)) || ! String.IsNullOrEmpty(StringUtil.RTrim( AV7auxResultado)) )
         {
            if ( String.IsNullOrEmpty(StringUtil.RTrim( AV6Operacao)) )
            {
               AV7auxResultado = AV5Resultado;
               AssignAttri("", false, "AV7auxResultado", AV7auxResultado);
               AV5Resultado = "";
               AssignAttri("", false, "AV5Resultado", AV5Resultado);
            }
            if ( StringUtil.StrCmp(AV6Operacao, "^") == 0 )
            {
               /* Execute user subroutine: 'BOTAORESULTADO' */
               S112 ();
               if (returnInSub) return;
            }
            else
            {
               AV6Operacao = "^";
               AssignAttri("", false, "AV6Operacao", AV6Operacao);
            }
            /* Execute user subroutine: 'MOSTRARUSUARIO' */
            S122 ();
            if (returnInSub) return;
         }
         else
         {
            GX_msglist.addItem("Por favor, insira o primeiro número");
         }
         /*  Sending Event outputs  */
      }

      protected void nextLoad( )
      {
      }

      protected void E32032( )
      {
         /* Load Routine */
         returnInSub = false;
      }

      protected void wb_table5_79_032( bool wbgen )
      {
         if ( wbgen )
         {
            /* Table start */
            sStyleString = "";
            GxWebStd.gx_table_start( context, tblTable1_Internalname, tblTable1_Internalname, "", "Table", 0, "", "", 1, 2, sStyleString, "", "", 0);
            context.WriteHtmlText( "<tr>") ;
            context.WriteHtmlText( "<td>") ;
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 82,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttBotaoresultado_Internalname, "", "=", bttBotaoresultado_Jsonclick, 5, "=", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'CHAMARRESULTADO\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            context.WriteHtmlText( "</td>") ;
            context.WriteHtmlText( "</tr>") ;
            /* End of table */
            context.WriteHtmlText( "</table>") ;
            wb_table5_79_032e( true) ;
         }
         else
         {
            wb_table5_79_032e( false) ;
         }
      }

      protected void wb_table4_37_032( bool wbgen )
      {
         if ( wbgen )
         {
            /* Table start */
            sStyleString = "";
            GxWebStd.gx_table_start( context, tblTable3_Internalname, tblTable3_Internalname, "", "Table tableCalc", 0, "", "", 1, 2, sStyleString, "", "", 0);
            context.WriteHtmlText( "<tr>") ;
            context.WriteHtmlText( "<td>") ;
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 40,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttRaizquadrada_Internalname, "", "v", bttRaizquadrada_Jsonclick, 5, "v", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'RAIZQUADRADA\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            context.WriteHtmlText( "</td>") ;
            context.WriteHtmlText( "</tr>") ;
            /* End of table */
            context.WriteHtmlText( "</table>") ;
            wb_table4_37_032e( true) ;
         }
         else
         {
            wb_table4_37_032e( false) ;
         }
      }

      protected void wb_table3_32_032( bool wbgen )
      {
         if ( wbgen )
         {
            /* Table start */
            sStyleString = "";
            GxWebStd.gx_table_start( context, tblTable5_Internalname, tblTable5_Internalname, "", "Table", 0, "", "", 1, 2, sStyleString, "", "", 0);
            context.WriteHtmlText( "<tr>") ;
            context.WriteHtmlText( "<td>") ;
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 35,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttPotencia_Internalname, "", "Potencia", bttPotencia_Jsonclick, 5, "Potencia", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'POTENCIA\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            context.WriteHtmlText( "</td>") ;
            context.WriteHtmlText( "</tr>") ;
            /* End of table */
            context.WriteHtmlText( "</table>") ;
            wb_table3_32_032e( true) ;
         }
         else
         {
            wb_table3_32_032e( false) ;
         }
      }

      protected void wb_table2_27_032( bool wbgen )
      {
         if ( wbgen )
         {
            /* Table start */
            sStyleString = "";
            GxWebStd.gx_table_start( context, tblTable4_Internalname, tblTable4_Internalname, "", "Table", 0, "", "", 1, 2, sStyleString, "", "", 0);
            context.WriteHtmlText( "<tr>") ;
            context.WriteHtmlText( "<td>") ;
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 30,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttAoquadrado_Internalname, "", "X²", bttAoquadrado_Jsonclick, 5, "X²", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"E\\'AOQUADRADO\\'."+"'", TempTags, "", context.GetButtonType( ), "HLP_General/Calculadora.htm");
            context.WriteHtmlText( "</td>") ;
            context.WriteHtmlText( "</tr>") ;
            /* End of table */
            context.WriteHtmlText( "</table>") ;
            wb_table2_27_032e( true) ;
         }
         else
         {
            wb_table2_27_032e( false) ;
         }
      }

      protected void wb_table1_21_032( bool wbgen )
      {
         if ( wbgen )
         {
            /* Table start */
            sStyleString = "";
            GxWebStd.gx_table_start( context, tblTable2_Internalname, tblTable2_Internalname, "", "Table", 0, "", "", 1, 2, sStyleString, "", "", 0);
            context.WriteHtmlText( "<tr>") ;
            context.WriteHtmlText( "<td>") ;
            TempTags = "  onfocus=\"gx.evt.onfocus(this, 24,'',false,'',0)\"";
            ClassString = "Button corFundo";
            StyleString = "";
            GxWebStd.gx_button_ctrl( context, bttApagarulitmonumero_Internalname, "", "<=", bttApagarulitmonumero_Jsonclick, 7, "<=", "", StyleString, ClassString, 1, 1, "standard", "'"+""+"'"+",false,"+"'"+"e33031_client"+"'", TempTags, "", 2, "HLP_General/Calculadora.htm");
            context.WriteHtmlText( "</td>") ;
            context.WriteHtmlText( "</tr>") ;
            /* End of table */
            context.WriteHtmlText( "</table>") ;
            wb_table1_21_032e( true) ;
         }
         else
         {
            wb_table1_21_032e( false) ;
         }
      }

      public override void setparameters( Object[] obj )
      {
         createObjects();
         initialize();
      }

      public override string getresponse( string sGXDynURL )
      {
         initialize_properties( ) ;
         BackMsgLst = context.GX_msglist;
         context.GX_msglist = LclMsgLst;
         sDynURL = sGXDynURL;
         nGotPars = (short)(1);
         nGXWrapped = (short)(1);
         context.SetWrapped(true);
         PA032( ) ;
         WS032( ) ;
         WE032( ) ;
         this.cleanup();
         context.SetWrapped(false);
         context.GX_msglist = BackMsgLst;
         return "";
      }

      public void responsestatic( string sGXDynURL )
      {
      }

      protected void define_styles( )
      {
         AddThemeStyleSheetFile("", context.GetTheme( )+".css", "?"+GetCacheInvalidationToken( ));
         bool outputEnabled = isOutputEnabled( );
         if ( context.isSpaRequest( ) )
         {
            enableOutput();
         }
         idxLst = 1;
         while ( idxLst <= Form.Jscriptsrc.Count )
         {
            context.AddJavascriptSource(StringUtil.RTrim( ((string)Form.Jscriptsrc.Item(idxLst))), "?20254101650107", true, true);
            idxLst = (int)(idxLst+1);
         }
         if ( ! outputEnabled )
         {
            if ( context.isSpaRequest( ) )
            {
               disableOutput();
            }
         }
         /* End function define_styles */
      }

      protected void include_jscripts( )
      {
         context.AddJavascriptSource("messages.eng.js", "?"+GetCacheInvalidationToken( ), false, true);
         context.AddJavascriptSource("gxdec.js", "?"+context.GetBuildNumber( 167520), false, true);
         context.AddJavascriptSource("general/calculadora.js", "?20254101650107", false, true);
         /* End function include_jscripts */
      }

      protected void init_web_controls( )
      {
         /* End function init_web_controls */
      }

      protected void init_default_properties( )
      {
         lblTextblock1_Internalname = "TEXTBLOCK1";
         edtavHistorico_Internalname = "vHISTORICO";
         edtavMostraruser_Internalname = "vMOSTRARUSER";
         bttCancelartudo_Internalname = "CANCELARTUDO";
         bttApagarulitmonumero_Internalname = "APAGARULITMONUMERO";
         tblTable2_Internalname = "TABLE2";
         bttAoquadrado_Internalname = "AOQUADRADO";
         tblTable4_Internalname = "TABLE4";
         bttPotencia_Internalname = "POTENCIA";
         tblTable5_Internalname = "TABLE5";
         bttRaizquadrada_Internalname = "RAIZQUADRADA";
         tblTable3_Internalname = "TABLE3";
         bttAdd7_Internalname = "ADD7";
         bttAdd8_Internalname = "ADD8";
         bttAdd9_Internalname = "ADD9";
         bttDividir_Internalname = "DIVIDIR";
         bttAdd4_Internalname = "ADD4";
         bttAdd5_Internalname = "ADD5";
         bttAdd6_Internalname = "ADD6";
         bttMultiplicar_Internalname = "MULTIPLICAR";
         bttAdd1_Internalname = "ADD1";
         bttAdd2_Internalname = "ADD2";
         bttAdd3_Internalname = "ADD3";
         bttSubtrair_Internalname = "SUBTRAIR";
         bttInvertersinal_Internalname = "INVERTERSINAL";
         bttAdd0_Internalname = "ADD0";
         bttAddvirgula_Internalname = "ADDVIRGULA";
         bttAdicionar_Internalname = "ADICIONAR";
         bttBotaoresultado_Internalname = "BOTAORESULTADO";
         tblTable1_Internalname = "TABLE1";
         divMaintable_Internalname = "MAINTABLE";
         Form.Internalname = "FORM";
      }

      public override void initialize_properties( )
      {
         context.SetDefaultTheme("Calculadora_Bruno", true);
         if ( context.isSpaRequest( ) )
         {
            disableJsOutput();
         }
         init_default_properties( ) ;
         edtavMostraruser_Jsonclick = "";
         edtavMostraruser_Enabled = 1;
         edtavHistorico_Jsonclick = "";
         edtavHistorico_Enabled = 1;
         Form.Headerrawhtml = "";
         Form.Background = "";
         Form.Textcolor = 0;
         Form.Backcolor = (int)(0xFFFFFF);
         Form.Caption = "Calculadora";
         if ( context.isSpaRequest( ) )
         {
            enableJsOutput();
         }
      }

      public override bool SupportAjaxEvent( )
      {
         return true ;
      }

      public override void InitializeDynEvents( )
      {
         setEventMetadata("REFRESH","{handler:'Refresh',iparms:[]");
         setEventMetadata("REFRESH",",oparms:[]}");
         setEventMetadata("'DIVIDIR'","{handler:'E23032',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]");
         setEventMetadata("'DIVIDIR'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''}]}");
         setEventMetadata("'MULTIPLICAR'","{handler:'E24032',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]");
         setEventMetadata("'MULTIPLICAR'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''}]}");
         setEventMetadata("'SUBTRAIR'","{handler:'E25032',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]");
         setEventMetadata("'SUBTRAIR'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''}]}");
         setEventMetadata("'ADICIONAR'","{handler:'E26032',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]");
         setEventMetadata("'ADICIONAR'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''}]}");
         setEventMetadata("'AOQUADRADO'","{handler:'E27032',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''}]");
         setEventMetadata("'AOQUADRADO'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''}]}");
         setEventMetadata("'RAIZQUADRADA'","{handler:'E28032',iparms:[{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''}]");
         setEventMetadata("'RAIZQUADRADA'",",oparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''}]}");
         setEventMetadata("'TROCARSINAL'","{handler:'E29032',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''}]");
         setEventMetadata("'TROCARSINAL'",",oparms:[{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD,'","{handler:'E22031',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD,'",",oparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'CANCELAR TUDO'","{handler:'E11031',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'CANCELAR TUDO'",",oparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'APAGARULITMONUMERO'","{handler:'E33031',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'APAGARULITMONUMERO'",",oparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'CHAMARRESULTADO'","{handler:'E30032',iparms:[{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''}]");
         setEventMetadata("'CHAMARRESULTADO'",",oparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''}]}");
         setEventMetadata("'ADD9'","{handler:'E14031',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD9'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD8'","{handler:'E13031',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD8'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD7'","{handler:'E12031',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD7'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD6'","{handler:'E17031',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD6'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD5'","{handler:'E16031',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD5'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD4'","{handler:'E15031',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD4'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD3'","{handler:'E20031',iparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD3'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD2'","{handler:'E19031',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD2'",",oparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD1'","{handler:'E18031',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD1'",",oparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'ADD0'","{handler:'E21031',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''}]");
         setEventMetadata("'ADD0'",",oparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]}");
         setEventMetadata("'POTENCIA'","{handler:'E31032',iparms:[{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''}]");
         setEventMetadata("'POTENCIA'",",oparms:[{av:'AV12CalculoSucesso',fld:'vCALCULOSUCESSO',pic:''},{av:'AV7auxResultado',fld:'vAUXRESULTADO',pic:''},{av:'AV5Resultado',fld:'vRESULTADO',pic:''},{av:'AV6Operacao',fld:'vOPERACAO',pic:''},{av:'AV13MostrarUser',fld:'vMOSTRARUSER',pic:''},{av:'AV27historico',fld:'vHISTORICO',pic:''}]}");
         return  ;
      }

      public override void cleanup( )
      {
         CloseCursors();
         if ( IsMain )
         {
            context.CloseConnections();
         }
      }

      public override void initialize( )
      {
         gxfirstwebparm = "";
         gxfirstwebparm_bkp = "";
         sDynURL = "";
         FormProcess = "";
         bodyStyle = "";
         GXKey = "";
         AV5Resultado = "";
         AV7auxResultado = "";
         AV6Operacao = "";
         AV12CalculoSucesso = false;
         GX_FocusControl = "";
         Form = new GXWebForm();
         sPrefix = "";
         lblTextblock1_Jsonclick = "";
         TempTags = "";
         AV27historico = "";
         AV13MostrarUser = "";
         ClassString = "";
         StyleString = "";
         bttCancelartudo_Jsonclick = "";
         bttAdd7_Jsonclick = "";
         bttAdd8_Jsonclick = "";
         bttAdd9_Jsonclick = "";
         bttDividir_Jsonclick = "";
         bttAdd4_Jsonclick = "";
         bttAdd5_Jsonclick = "";
         bttAdd6_Jsonclick = "";
         bttMultiplicar_Jsonclick = "";
         bttAdd1_Jsonclick = "";
         bttAdd2_Jsonclick = "";
         bttAdd3_Jsonclick = "";
         bttSubtrair_Jsonclick = "";
         bttInvertersinal_Jsonclick = "";
         bttAdd0_Jsonclick = "";
         bttAddvirgula_Jsonclick = "";
         bttAdicionar_Jsonclick = "";
         sEvt = "";
         EvtGridId = "";
         EvtRowId = "";
         sEvtType = "";
         AV18expressionDataTypeVariable = new ExpressionEvaluator(context);
         sStyleString = "";
         bttBotaoresultado_Jsonclick = "";
         bttRaizquadrada_Jsonclick = "";
         bttPotencia_Jsonclick = "";
         bttAoquadrado_Jsonclick = "";
         bttApagarulitmonumero_Jsonclick = "";
         BackMsgLst = new msglist();
         LclMsgLst = new msglist();
         /* GeneXus formulas. */
         edtavHistorico_Enabled = 0;
         edtavMostraruser_Enabled = 0;
      }

      private short nGotPars ;
      private short GxWebError ;
      private short gxajaxcallmode ;
      private short wbEnd ;
      private short wbStart ;
      private short nDonePA ;
      private short gxcookieaux ;
      private short AV15Count ;
      private short nGXWrapped ;
      private int edtavHistorico_Enabled ;
      private int edtavMostraruser_Enabled ;
      private int idxLst ;
      private decimal AV11numero1 ;
      private decimal AV10numero2 ;
      private string gxfirstwebparm ;
      private string gxfirstwebparm_bkp ;
      private string sDynURL ;
      private string FormProcess ;
      private string bodyStyle ;
      private string GXKey ;
      private string GX_FocusControl ;
      private string sPrefix ;
      private string divMaintable_Internalname ;
      private string lblTextblock1_Internalname ;
      private string lblTextblock1_Jsonclick ;
      private string edtavHistorico_Internalname ;
      private string TempTags ;
      private string edtavHistorico_Jsonclick ;
      private string edtavMostraruser_Internalname ;
      private string edtavMostraruser_Jsonclick ;
      private string ClassString ;
      private string StyleString ;
      private string bttCancelartudo_Internalname ;
      private string bttCancelartudo_Jsonclick ;
      private string bttAdd7_Internalname ;
      private string bttAdd7_Jsonclick ;
      private string bttAdd8_Internalname ;
      private string bttAdd8_Jsonclick ;
      private string bttAdd9_Internalname ;
      private string bttAdd9_Jsonclick ;
      private string bttDividir_Internalname ;
      private string bttDividir_Jsonclick ;
      private string bttAdd4_Internalname ;
      private string bttAdd4_Jsonclick ;
      private string bttAdd5_Internalname ;
      private string bttAdd5_Jsonclick ;
      private string bttAdd6_Internalname ;
      private string bttAdd6_Jsonclick ;
      private string bttMultiplicar_Internalname ;
      private string bttMultiplicar_Jsonclick ;
      private string bttAdd1_Internalname ;
      private string bttAdd1_Jsonclick ;
      private string bttAdd2_Internalname ;
      private string bttAdd2_Jsonclick ;
      private string bttAdd3_Internalname ;
      private string bttAdd3_Jsonclick ;
      private string bttSubtrair_Internalname ;
      private string bttSubtrair_Jsonclick ;
      private string bttInvertersinal_Internalname ;
      private string bttInvertersinal_Jsonclick ;
      private string bttAdd0_Internalname ;
      private string bttAdd0_Jsonclick ;
      private string bttAddvirgula_Internalname ;
      private string bttAddvirgula_Jsonclick ;
      private string bttAdicionar_Internalname ;
      private string bttAdicionar_Jsonclick ;
      private string sEvt ;
      private string EvtGridId ;
      private string EvtRowId ;
      private string sEvtType ;
      private string sStyleString ;
      private string tblTable1_Internalname ;
      private string bttBotaoresultado_Internalname ;
      private string bttBotaoresultado_Jsonclick ;
      private string tblTable3_Internalname ;
      private string bttRaizquadrada_Internalname ;
      private string bttRaizquadrada_Jsonclick ;
      private string tblTable5_Internalname ;
      private string bttPotencia_Internalname ;
      private string bttPotencia_Jsonclick ;
      private string tblTable4_Internalname ;
      private string bttAoquadrado_Internalname ;
      private string bttAoquadrado_Jsonclick ;
      private string tblTable2_Internalname ;
      private string bttApagarulitmonumero_Internalname ;
      private string bttApagarulitmonumero_Jsonclick ;
      private bool entryPointCalled ;
      private bool toggleJsOutput ;
      private bool AV12CalculoSucesso ;
      private bool wbLoad ;
      private bool Rfr0gs ;
      private bool wbErr ;
      private bool gxdyncontrolsrefreshing ;
      private bool returnInSub ;
      private string AV5Resultado ;
      private string AV7auxResultado ;
      private string AV6Operacao ;
      private string AV27historico ;
      private string AV13MostrarUser ;
      private IGxDataStore dsDefault ;
      private msglist BackMsgLst ;
      private msglist LclMsgLst ;
      private GXWebForm Form ;
      private ExpressionEvaluator AV18expressionDataTypeVariable ;
   }

}
