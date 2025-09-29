using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace DapperCRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // DataGridView düzenleme işlemleri
        void DataGridViewDüzenle()
        {
            // Otomatik boyutlandırma
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        // TextBox'ların temizlenmesi
        void ClearTextBox()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }

        // Listeden Seçme
        void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {  
            if(e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(); //id
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(); //isim
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(); //fiyat

                textBox5.ReadOnly = false;
            }
        }
        
        // Veritabanı bağlantısı
        const string ConnectionString = @"Data Source=MertPC\SQLEXPRESS;Initial Catalog=Store;Integrated Security=True";

        // Listeleme Butonu
        async void button3_Click(object sender, EventArgs e)
        {
            // CRUD işlemlerini yapacak repository sınıfımızın örneği.
            var repository = new ProductRepository(ConnectionString);

            var allProducts = await repository.GetAllAsync();

            dataGridView1.DataSource = allProducts.ToList();

            DataGridViewDüzenle();
        }

        // Arama Butonu
        async void button2_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBox3.Text))
            {
                // CRUD işlemlerini yapacak repository sınıfımızın örneği.
                var repository = new ProductRepository(ConnectionString);
                int id = int.Parse(textBox3.Text);
                var product = await repository.GetByIdAsync(id);

                if (product != null)
                {
                    dataGridView1.DataSource = new List<Product> { product };
                    DataGridViewDüzenle();
                }
                else
                {
                    dataGridView1.Columns.Clear();
                    MessageBox.Show("Ürün bulunamadı.");
                }

            }
            else
            {
                MessageBox.Show("Lütfen ID alanını doldurunuz.");
            }
        }

        // Ekleme Butonu
        async void button1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                // CRUD işlemlerini yapacak repository sınıfımızın örneği.
                var repository = new ProductRepository(ConnectionString);

                string name = textBox1.Text;
                decimal price = decimal.Parse(textBox2.Text);

                var newProduct = new Product { Name_ = name, Price = price };
                var addedRows = await repository.CreateAsync(newProduct);

                ClearTextBox(); // TextBox'ları temizle
                button3_Click(sender,e); // tekrardan listeleme yapmak için
                MessageBox.Show("Ürün eklenimi başarıyla gerçekleşti.");
            }
            else
            {
                MessageBox.Show("Lütfen boş alanları doldurunuz.");
            }
        }

        // Güncelle Butonu
        async void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox6.Text))
            {
                // CRUD işlemlerini yapacak repository sınıfımızın örneği.
                var repository = new ProductRepository(ConnectionString);

                int id = int.Parse(textBox6.Text);
                string name = textBox4.Text;
                decimal price = decimal.Parse(textBox5.Text);

                var updatedProduct = new Product { ID = id, Name_ = name, Price = price };

                var updatedRows = await repository.UpdateAsync(updatedProduct);

                if (updatedRows > 0)
                {
                    MessageBox.Show("Ürün güncelleme işlemi başarılı.");
                    button3_Click(sender, e); // Listeleme butonunu çağırarak güncellenmiş veriyi göster
                    ClearTextBox();
                }
                else
                {
                    MessageBox.Show("Güncelleme işlemi başarısız.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen boş alanları doldurunuz.");
            }
        }

        // Silme Butonu
        async void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox4.Text) && !string.IsNullOrEmpty(textBox5.Text) && !string.IsNullOrEmpty(textBox6.Text))
            {
                // CRUD işlemlerini yapacak repository sınıfımızın örneği.
                var repository = new ProductRepository(ConnectionString);

                int id = int.Parse(textBox6.Text);

                var deletedRows = await repository.DeleteAsync(id);

                if (deletedRows > 0)
                {
                    MessageBox.Show("Ürün silme işlemi başarılı.");
                    button3_Click(sender, e);
                    ClearTextBox();
                }
                else
                {
                    MessageBox.Show("Silme işlemi başarısız.");
                }
            }
            else
            {
                MessageBox.Show("Lütfen boş alanları doldurunuz.");
            }
        }
    }
}