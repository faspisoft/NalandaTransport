/***
 * 
 * Componente: CDSImprimeTexto
 *		  Autor: Carlos dos Santos
 *		   Data: 21/07/2004
 *      Revisão: 25/04/2011 (este revisão modifica o acesso a porta da impressora, permitindo que o componente funcione no Windows Vista e Windows 7.
 *   Objetivo: Imprime texto diretamente na porta da impressora.
 * 
 ***/
using System;
using System.Runtime.InteropServices;
using System.IO;

namespace faspi
{
	class PortaException : System.Exception {};
  
	/// <summary>
	/// Classe para impressão de textos diretamente para a porta da impressora.
	/// © 2004 CDS Informática Ltda.
	/// </summary>
	public class DOSPrint
	{

		private int GENERIC_WRITE = 0x40000000;
		private int OPEN_EXISTING = 3;
		private int FILE_SHARE_WRITE = 0x2;
		private string sPorta;
		private int hPort;
		private FileStream outFile;
		private StreamWriter fileWriter;
		private IntPtr hPortP;
		private bool lOK = false;
        private string GeraArquivoLPT;

		private string Chr(int asc)
		{
			string ret = "";
			ret += (char)asc;
			return ret;
		}

		[DllImport("kernel32.dll",EntryPoint="CreateFileA")]
		static extern int CreateFileA(string lpFileName,int dwDesiredAccess, int dwShareMode,
			int lpSecurityAttributes,
			int dwCreationDisposition, int dwFlagsAndAttributes,
			int hTemplateFile);

		[DllImport("kernel32.dll",EntryPoint="CloseHandle")]
		static extern int CloseHandle(int hObject);

		
		public string Normal
		{
			get 
			{
					return Chr(18);
			}
		}

		
		public string CondensedOn
		{
			get 
			{
				return Chr(15);
			}
		}

		
		public string Expandido
		{
			get 
			{
				return Chr(14);
			}
		}

		
		public string ExpandidoNormal
		{
			get 
			{
				return Chr(20);
			}
		}


		
		public string BoldOn
		{
			get 
			{
				return Chr(27) + Chr(69);
			}
		}

        public string SetPageSize6Inch
		{
			get 
			{
                return Chr(27) +  Chr(67) + Chr(0) + Chr(5);
			}
		}
        

		
        public string BoldOff
		{
			get 
			{
				return Chr(27) + Chr(70);
			}
		}

		//heading
        public string HeadingOn
        {
            get
            {
                return Chr(27) + Chr(87) + Chr(49);
            }
        }

        public string HeadingOff
        {
            get
            {
                return Chr(27) + Chr(87) + Chr(48);
            }
        }

        //underline
        public string UnderlineOn
        {
            get
            {
                return Chr(27) + Chr(45) + Chr(49);
            }
        }

        public string UnderlineOff
        {
            get
            {
                return Chr(27) + Chr(45) + Chr(48);
            }
        }

        //large font
        public string LargeFont
        {
            get
            {
                return Chr(27) + Chr(80);
            }
        }

        //small font
        public string SmallFont
        {
            get
            {
                return Chr(27) + Chr(77);
            }
        }
		public bool Inicio(string sPortaInicio)
		{
            GeraArquivoLPT = "";
			sPortaInicio.ToUpper();
            outFile = null;
            if (sPortaInicio.Substring(0, 3) == "LPT")
            {
                if (sPortaInicio == "LPT")
                {
                    sPortaInicio = "LPT1";
                }
                sPorta = sPortaInicio;
                sPortaInicio = "LPT-" + DateTime.Now.Hour.ToString("00") + DateTime.Now.Minute.ToString("00") + DateTime.Now.Second.ToString("00") + ".TXT";
                GeraArquivoLPT = sPortaInicio;
                fileWriter = new StreamWriter(sPortaInicio);
                lOK = true;
               
            }
            else
            {
                fileWriter = new StreamWriter(sPortaInicio);
                lOK = true;
            }
			return lOK;
		}

		
		public void Fim()
		{
			if(lOK)
			{
				fileWriter.Close();
                if (outFile != null)
                {
                    outFile.Close();
                    CloseHandle(hPort);
                }

				lOK = false;

                if (GeraArquivoLPT != String.Empty)
                {
                    System.IO.File.Copy(GeraArquivoLPT, sPorta);
                   // System.IO.File.Copy(GeraArquivoLPT, "C:\\Users\\abc\\Desktop\\printformat.TXT");
                    File.Delete(GeraArquivoLPT);
                    GeraArquivoLPT = "";
                }
			}
		}

		public void Imp(string sLinha)
		{
			if(lOK) 
			{
				fileWriter.Write(sLinha);
				fileWriter.Flush();
			}
		}  
		
		
		public void ImpLF(string sLinha)
		{
			if(lOK)
			{
				fileWriter.Flush();
			}
		}  

		
		public void ImpCol(int nCol, string sLinha)
		{
            string Cols = "";
            Cols = Cols.PadLeft(nCol, ' ');
            Imp(Chr(13)+ Cols + sLinha);
		}
	
    
		public void ImpColLF(int nCol, string sLinha)
		{
            ImpCol(nCol, sLinha);
            LineSpace(1);
		}

		
		public void LineSpace(int nLinha)
		{
			for(int i=0;i<nLinha;i++)
			{
				ImpLF("");
			}
	    }

        public void NextLine()
        {
            //return Chr(13);
            Imp(Environment.NewLine);

        }

		public void Eject()
		{
			Imp(Chr(12));
		}

        public DOSPrint()
		{
			sPorta = "LPT1";
		}

        public string Line()
        {
            string str = "";
            for (int i = 1; i <= 80; i++)
            {
               str+= "=";
            }
            return str;
        }

        public string WriteL(string sLinha, int L)
        {
            string str = "";
            int i = sLinha.Trim().Length;
            if (lOK)
            {
                if (L == 0)
                {
                    L = i;
                }
                if ((L - i) >= 0)
                {
                    fileWriter.Write(sLinha);
                    for (int j = 1; j <= (L - i); j++)
                    {
                        str += " ";
                        //fileWriter.Write(" ");
                    }
                }
                else
                {
                    str += sLinha.Substring(1,L);
                    
                }

            }
            return str;
        }

        public void WriteR(string sLinha, int L)
        {
            int i = sLinha.Trim().Length;
            if (lOK)
            {
                //if (L == 0)
                //{
                //    L = i;
                //}
                if ((L - i) >= 0)
                {
                    for (int j = 1; j <= (L - i); j++)
                    {
                        fileWriter.Write(" ");
                    }
                    fileWriter.Write(sLinha);

                }
                else
                {
                    fileWriter.Write(sLinha);
                }


                fileWriter.Flush();
            }
        }

        public string WriteM(string sLinha, int L)
        {
            string str = "";
            int i = sLinha.Trim().Length;
            if (lOK)
            {
                int N;

                if ((L - i) % 2 == 1)
                {
                    N = L - i - 1;

                }
                else
                {
                    N = L - i;
                }

                if ((L - i) >= 0)
                {
                    for (int j = 1; j <= N / 2; j++)
                    {
                        str += " ";
                        //fileWriter.Write(" ");
                        
                    }
                    str += sLinha;
                    //fileWriter.Write(sLinha);
                    for (int j = 1; j <= N / 2; j++)
                    {
                        str += " ";
                        //fileWriter.Write(" ");
                    }
                }
                else
                {
                    str += sLinha;
                    //fileWriter.Write(sLinha);
                }
                
                //fileWriter.Flush();
            }
            return str;
        }
        public string GetRightFormatedText(string Cont, int Length)
        {
            int rLoc = Cont.Trim().Length;
            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                string space = "";

                //for (nos = 0; nos <= (Length - rLoc); nos++)
                //{
                //    space += " ";
                //}
                //Cont = space + Cont;

                for (nos = 0; nos <= (Length - rLoc); nos++)
                {
                    space += " ";
                }
                Cont = space + Cont;

                //for (int j = 1; j <= (L - i); j++)
                //{
                //    fileWriter.Write(" ");
                //}
                //fileWriter.Write(sLinha);
            }
            return (Cont);
        }
        public string GetFormatedText(string Cont, int Length)
        {
            int rLoc = Length - Cont.Length;
            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                for (nos = 0; nos <= rLoc / 2; nos++)
                {
                    Cont = Cont + " ";
                }

            }
            return (Cont);
        }
        public static string GetFixedLengthString(string input, int length)
        {

            string result = string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                result = new string(' ', length);
            }
            else if (input.Length > length)
            {
                result = input.Substring(0, length);
            }
            else
            {
                result = input.PadRight(length);
            }

            return result;
        }

        public string GetCenterdFormatedText(string Cont, int Length)
        {
            int rLoc = Length - Cont.Length;
            if (rLoc < 0)
            {
                Cont = Cont.Substring(0, Length);
            }
            else
            {
                int nos;
                string space = "";
                for (nos = 0; nos <= rLoc / 2; nos++)
                {
                    space += " ";
                }
                Cont = space + Cont;
            }
            return (Cont);
        }




    }
}
