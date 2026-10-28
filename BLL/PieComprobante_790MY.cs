using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BLL_08YS
{
    /// <summary>Pie del comprobante de reserva (ReservaBLL_790MY.Comprobante): línea punteada, datos de emisión y agradecimiento.</summary>
    internal sealed class PieComprobante_790MY : PdfPageEventHelper
    {
        private readonly string _emitido;
        private readonly string _gracias;
        private readonly Font _fPie;
        private readonly Font _fGracias;

        public PieComprobante_790MY(string emitido, string gracias, Font fPie, Font fGracias)
        {
            _emitido  = emitido;
            _gracias  = gracias;
            _fPie     = fPie;
            _fGracias = fGracias;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfContentByte cb = writer.DirectContent;
            float ancho = document.PageSize.Width;

            cb.SaveState();
            cb.SetColorStroke(ReservaBLL_790MY.PdfTileBorde);
            cb.SetLineWidth(1f);
            cb.SetLineDash(1.5f, 3f, 0f);
            cb.MoveTo(30f, 58f);
            cb.LineTo(ancho - 30f, 58f);
            cb.Stroke();
            cb.RestoreState();

            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(_emitido, _fPie), ancho / 2f, 42f, 0);
            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(_gracias, _fGracias), ancho / 2f, 26f, 0);
        }
    }
}
