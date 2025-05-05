using System;
using System.Collections;
using GeneXus.Utils;
using GeneXus.Resources;
using GeneXus.Application;
using GeneXus.Metadata;
using GeneXus.Cryptography;
using com.genexus;
using GeneXus.Data.ADO;
using GeneXus.Data.NTier;
using GeneXus.Data.NTier.ADO;
using GeneXus.WebControls;
using GeneXus.Http;
using GeneXus.Procedure;
using GeneXus.XML;
using GeneXus.Search;
using GeneXus.Encryption;
using GeneXus.Http.Client;
using System.Threading;
using System.Xml.Serialization;
using System.Runtime.Serialization;
namespace GeneXus.Programs.general {
   public class checavazio : GXProcedure
   {
      public checavazio( )
      {
         context = new GxContext(  );
         DataStoreUtil.LoadDataStores( context);
         IsMain = true;
         context.SetDefaultTheme("Calculadora_Bruno", true);
      }

      public checavazio( IGxContext context )
      {
         this.context = context;
         IsMain = false;
      }

      public void execute( ref string aP0_checaVazio )
      {
         this.AV8checaVazio = aP0_checaVazio;
         initialize();
         ExecuteImpl();
         aP0_checaVazio=this.AV8checaVazio;
      }

      public string executeUdp( )
      {
         execute(ref aP0_checaVazio);
         return AV8checaVazio ;
      }

      public void executeSubmit( ref string aP0_checaVazio )
      {
         this.AV8checaVazio = aP0_checaVazio;
         SubmitImpl();
         aP0_checaVazio=this.AV8checaVazio;
      }

      protected override void ExecutePrivate( )
      {
         /* GeneXus formulas */
         /* Output device settings */
         if ( String.IsNullOrEmpty(StringUtil.RTrim( AV8checaVazio)) )
         {
            AV8checaVazio = StringUtil.Str( (decimal)(1), 10, 0);
         }
         else
         {
            AV8checaVazio = StringUtil.Str( (decimal)(0), 10, 0);
         }
         this.cleanup();
      }

      public override void cleanup( )
      {
         CloseCursors();
         if ( IsMain )
         {
            context.CloseConnections();
         }
         ExitApp();
      }

      public override void initialize( )
      {
         /* GeneXus formulas. */
      }

      private string AV8checaVazio ;
      private string aP0_checaVazio ;
   }

}
