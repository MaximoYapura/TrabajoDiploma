using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GUI_08YS.RF2
{
    /// <summary>Cursor de escritura del ticket de FormComprobanteFactura_790MY: posición vertical, márgenes y fuentes.</summary>
    internal sealed class TicketFactura_790MY : IDisposable
    {
        private const float MargenLateral = 22f;

        public Graphics G { get; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Ancho { get; private set; }

        public readonly Font FCuerpo = new Font("Courier New", 9.5f);
        public readonly Font FChica = new Font("Courier New", 8.5f);
        public readonly Font FMini = new Font("Courier New", 7.5f);
        public readonly Font FNegrita = new Font("Courier New", 9.5f, FontStyle.Bold);
        public readonly Font FNegritaChica = new Font("Courier New", 8.5f, FontStyle.Bold);
        public readonly Font FTitulo = new Font("Courier New", 12f, FontStyle.Bold);
        public readonly Font FTotal = new Font("Courier New", 15f, FontStyle.Bold);
        public readonly Font FLetra = new Font("Courier New", 18f, FontStyle.Bold);
        public readonly Font FSello = new Font("Courier New", 24f, FontStyle.Bold);
        public readonly Font FMarca = new Font("Georgia", 20f, FontStyle.Bold);
        public readonly SolidBrush Pincel = new SolidBrush(FormComprobanteFactura_790MY.Tinta);
        public readonly SolidBrush PincelSuave = new SolidBrush(FormComprobanteFactura_790MY.TintaSuave);

        private readonly StringFormat _centro = new StringFormat { Alignment = StringAlignment.Center };
        private readonly StringFormat _derecha = new StringFormat { Alignment = StringAlignment.Far };

        public TicketFactura_790MY(Graphics g, float x, float y, float anchoTotal)
        {
            G = g;
            X = x + MargenLateral;
            Y = y;
            Ancho = anchoTotal - 2 * MargenLateral;
        }

        public void Espacio(float alto) => Y += alto;

        /// <summary>Achica (positivo) o restaura (negativo) el área de escritura.</summary>
        public void Margen(float delta)
        {
            X += delta;
            Ancho -= 2 * delta;
        }

        public void Centrado(string texto, Font fuente, bool suave = false)
        {
            var area = new RectangleF(X, Y, Ancho, 1000);
            SizeF med = G.MeasureString(texto, fuente, (int)Ancho, _centro);
            G.DrawString(texto, fuente, suave ? PincelSuave : Pincel, area, _centro);
            Y += med.Height + 2;
        }

        public void Izquierda(string texto, Font fuente, bool suave = false)
        {
            SizeF med = G.MeasureString(texto, fuente, (int)Ancho);
            G.DrawString(texto, fuente, suave ? PincelSuave : Pincel, new RectangleF(X, Y, Ancho, med.Height + 2));
            Y += med.Height + 2;
        }

        public void Fila(string izquierda, string derecha, bool negritaIzq = false, bool negritaDer = false)
        {
            Font fi = negritaIzq ? FNegrita : FCuerpo;
            Font fd = negritaDer ? FNegrita : FCuerpo;
            float alto = Math.Max(G.MeasureString(izquierda, fi).Height, G.MeasureString(derecha, fd).Height);
            G.DrawString(izquierda, fi, Pincel, X, Y);
            G.DrawString(derecha, fd, Pincel, new RectangleF(X, Y, Ancho, alto + 2), _derecha);
            Y += alto + 3;
        }

        public void FilaGrande(string izquierda, string derecha)
        {
            SizeF med = G.MeasureString(derecha, FTotal);
            G.DrawString(izquierda, FNegrita, Pincel, X, Y + (med.Height - G.MeasureString(izquierda, FNegrita).Height) / 2f + 2);
            G.DrawString(derecha, FTotal, Pincel, new RectangleF(X, Y, Ancho, med.Height + 2), _derecha);
            Y += med.Height + 2;
        }

        /// <summary>Cant | Descripción (con salto de línea) | P.Unit | Subt.</summary>
        public void Columnas(string cant, string descripcion, string precio, string subtotal, Font fuente)
        {
            float wCant = 36, wPrecio = 78, wSub = 78;
            float wDesc = Ancho - wCant - wPrecio - wSub - 8;
            SizeF medDesc = G.MeasureString(descripcion, fuente, (int)wDesc);
            float alto = Math.Max(medDesc.Height, G.MeasureString("0", fuente).Height);

            G.DrawString(cant, fuente, Pincel, X, Y);
            G.DrawString(descripcion, fuente, Pincel, new RectangleF(X + wCant, Y, wDesc, alto + 2));
            G.DrawString(precio, fuente, Pincel, new RectangleF(X + wCant + wDesc, Y, wPrecio, alto), _derecha);
            G.DrawString(subtotal, fuente, Pincel, new RectangleF(X + Ancho - wSub, Y, wSub, alto), _derecha);
            Y += alto + 4;
        }

        public void Linea(Color color, float grosor)
        {
            using (var lapiz = new Pen(color, grosor))
                G.DrawLine(lapiz, X, Y, X + Ancho, Y);
        }

        public void Separador(bool grueso)
        {
            using (var lapiz = new Pen(FormComprobanteFactura_790MY.Tinta, grueso ? 2f : 1f) { DashStyle = DashStyle.Dash })
                G.DrawLine(lapiz, X, Y, X + Ancho, Y);
            Y += grueso ? 2 : 1;
        }

        public void Recuadro(string texto, Font fuente, float grosor, bool angosto = false)
        {
            float ancho = angosto ? Math.Min(Ancho, G.MeasureString(texto, fuente).Width + 24) : Ancho;
            float x = X + (Ancho - ancho) / 2f;
            SizeF med = G.MeasureString(texto, fuente, (int)(ancho - 20), _centro);
            var rect = new RectangleF(x, Y, ancho, med.Height + 16);
            using (var lapiz = new Pen(FormComprobanteFactura_790MY.Tinta, grosor))
                G.DrawRectangle(lapiz, rect.X, rect.Y, rect.Width, rect.Height);
            G.DrawString(texto, fuente, Pincel, new RectangleF(rect.X + 10, rect.Y + 8, rect.Width - 20, med.Height + 2), _centro);
            Y += rect.Height;
        }

        public void Tijera()
        {
            using (var simbolo = new Font("Segoe UI Symbol", 11f))
                G.DrawString("✂", simbolo, PincelSuave, X, Y - 11);
            using (var lapiz = new Pen(FormComprobanteFactura_790MY.TintaSuave, 1f) { DashStyle = DashStyle.Dash })
                G.DrawLine(lapiz, X + 22, Y, X + Ancho, Y);
        }

        public void Dispose()
        {
            foreach (var f in new[] { FCuerpo, FChica, FMini, FNegrita, FNegritaChica, FTitulo, FTotal, FLetra, FSello, FMarca })
                f.Dispose();
            Pincel.Dispose();
            PincelSuave.Dispose();
            _centro.Dispose();
            _derecha.Dispose();
        }
    }
}
