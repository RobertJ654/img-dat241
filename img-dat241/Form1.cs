using System.Runtime.Intrinsics.X86;

namespace img_dat241
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Clase con el formato COO
        private class MatrizCOO
        {
            public List<int> Filas { get; set; }
            public List<int> Columnas { get; set; }
            public List<int> Valores { get; set; }

            public MatrizCOO()
            {
                Filas = new List<int>();
                Columnas = new List<int>();
                Valores = new List<int>();
            }

            public void mostrar(TextBox textBox)
            {
                textBox.AppendText("Filas:" + Environment.NewLine); // Para el salto de línea
                for (int i = 0; i < Filas.Count; i++)
                {
                    textBox.AppendText(Filas[i] + " ");
                }
                textBox.AppendText(Environment.NewLine);

                textBox.AppendText("Columnas:" + Environment.NewLine);
                for (int i = 0; i < Columnas.Count; i++)
                {
                    textBox.AppendText(Columnas[i] + " ");
                }
                textBox.AppendText(Environment.NewLine);

                textBox.AppendText("Valores:" + Environment.NewLine);
                for (int i = 0; i < Valores.Count; i++)
                {
                    textBox.AppendText(Valores[i] + " ");
                }
                textBox.AppendText(Environment.NewLine);
            }
        }

        // Botones para las imágenes
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Archivos JPG|*.jpg|Archivos BMP|*.bmp";
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Archivos JPG|*.jpg|Archivos BMP|*.bmp";
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox2.Image = bmp;
        }


        // Botón para la fusión por matrices
        private void button4_Click(object sender, EventArgs e)
        {
            // Convertimos ambas matrices en formato COO (Formato de coordenadas)
            MatrizCOO mat1 = convertirAMatriz((Bitmap)pictureBox1.Image);
            MatrizCOO mat2 = convertirAMatriz((Bitmap)pictureBox2.Image);

            // Mostramos ambas salidas en el textbox
            textBox1.AppendText("MATRIZ 1 (IMAGEN 1):" + Environment.NewLine);
            mat1.mostrar(textBox1);
            textBox1.AppendText(Environment.NewLine + Environment.NewLine + Environment.NewLine);
            textBox1.AppendText("MATRIZ 2 (IMAGEN 2):" + Environment.NewLine);
            mat1.mostrar(textBox1);

            // Fusionamos ambas matrices de formato COO a una sola
            MatrizCOO matFinal = new MatrizCOO();
            matFinal = fusionarMatricesCOO(mat1, mat2);

            // mostramos en bitmap la matriz resultante:
            convertirABitmap(matFinal);
        }

        private MatrizCOO convertirAMatriz(Bitmap bmp)
        {
            MatrizCOO matrizCOO = new MatrizCOO();
            Color c = new Color();
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    if (c.R == 0 && c.G == 0 && c.B == 0)
                    {
                        // Almacena las coordenadas y el valor "1" en las listas del formato COO
                        matrizCOO.Filas.Add(j);
                        matrizCOO.Columnas.Add(i);
                        matrizCOO.Valores.Add(1);
                    }
                }
            }
            return matrizCOO;
        }

        private MatrizCOO fusionarMatricesCOO(MatrizCOO mat1, MatrizCOO mat2)
        {
            MatrizCOO matrizFusionada = new MatrizCOO();

            // Fusiona las listas de filas, columnas y valores de mat1
            matrizFusionada.Filas.AddRange(mat1.Filas); // unimos con AddRange la fila de mat1 a la matriz fusionada
            matrizFusionada.Columnas.AddRange(mat1.Columnas);
            matrizFusionada.Valores.AddRange(mat1.Valores);
            // Fusiona las listas de filas, columnas y valores de mat2
            matrizFusionada.Filas.AddRange(mat2.Filas); // unimos con AddRange la fila de mat2 a la matriz fusionada
            matrizFusionada.Columnas.AddRange(mat2.Columnas);
            matrizFusionada.Valores.AddRange(mat2.Valores);

            return matrizFusionada;
        }


        private void convertirABitmap(MatrizCOO matrizCoo)
        {
            // Obtener el tamaño de la imagen a partir de los valores máximos de filas y columnas
            int anchoMaximo = matrizCoo.Columnas.Max() + 1;
            int altoMaximo = matrizCoo.Filas.Max() + 1;

            // Crear un nuevo objeto Bitmap con el tamaño obtenido
            Bitmap bitmap = new Bitmap(anchoMaximo, altoMaximo);

            // Rellenar el Bitmap con los valores de la matriz COO
            for (int i = 0; i < matrizCoo.Filas.Count; i++)
            {
                int fila = matrizCoo.Filas[i];
                int columna = matrizCoo.Columnas[i];
                int valor = matrizCoo.Valores[i];

                // Si el valor es 1, establecer el píxel como rojo, de lo contrario, como negro
                Color color = (valor == 1) ? Color.Red : Color.Black;
                bitmap.SetPixel(columna, fila, color);
            }

            pictureBox3.Image = bitmap;
        }

    }
}
