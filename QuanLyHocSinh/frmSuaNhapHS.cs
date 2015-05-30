﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAO;
using System.Drawing.Drawing2D;


namespace QuanLyHocSinh
{
    public partial class frmSuaNhapHS : FormFlat
    {
        //them
        HOCSINH hs = new HOCSINH();
        PHANLOP pl = new PHANLOP();
        List<LOP> ll = new List<LOP>();
        HocSinh_BUS hs_bus= new HocSinh_BUS();
        bool m_check = true;
        string m_imagePath;

        public frmSuaNhapHS()
        {
            InitializeComponent();
            initGP();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;            

            m_ccbLop.DataSource = DataBase.Lop.LayDuLieuLop(FrmMain.m_phanquyen.ID);
            m_ccbLop.ValueMember = "MALOP";
            m_ccbLop.DisplayMember = "TENLOP";
            m_check = true;
            m_ccbManamhoc.DataSource = DataBase.NamHoc.LayNamHoc();
            m_ccbManamhoc.ValueMember = "MANAMHOC";
            m_ccbManamhoc.DisplayMember = "TENNAMHOC";
            m_ccbManamhoc.Visible = true;
            m_lblManamhoc.Visible = true;
        }

        //sua
        public frmSuaNhapHS(DataGridViewRow row)
        {
            InitializeComponent();
            initGP();

            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            m_txtMahs.Text = row.Cells["MAHS"].Value.ToString();
            m_tbDiaChi.Text = row.Cells["DIACHI"].Value.ToString();
            m_tbEmail.Text = row.Cells["EMAIL"].Value.ToString();
            m_tbGioiTinh.Text = row.Cells["GIOITINH"].Value.ToString();
            m_tbHoVaTen.Text = row.Cells["HOTEN"].Value.ToString();
            m_ccbLop.ValueMember = row.Cells["MALOP"].Value.ToString();
            m_ccbLop.DisplayMember = row.Cells["MALOP"].Value.ToString();
            m_dtpNgaysinh.Value =  DateTime.Parse(row.Cells["NGAYSINH"].Value.ToString());
            m_tbTonGiao.Text = row.Cells["TONGIAO"].Value.ToString();
            m_tbHotencha.Text = row.Cells["HOTENCHA"].Value.ToString();
            m_tbHotenme.Text = row.Cells["NGHENGHIEPCHA"].Value.ToString();
            m_tbNghenghiepcha.Text = row.Cells["HOTENME"].Value.ToString();
            m_tbNghenghiepme.Text = row.Cells["NGHENGHIEPME"].Value.ToString();
            if (DataBase.HocSinh.Image(int.Parse(m_txtMahs.Text)) != null)
                m_ptbHS.Image = (Image)(new Bitmap(DataBase.HocSinh.Image(int.Parse(m_txtMahs.Text))));

            LOP l = new LOP();
            l.MALOP = row.Cells["MALOP"].Value.ToString();
            ll.Add(l);
            m_ccbLop.DataSource = ll;
            m_ccbLop.ValueMember = "MALOP";
            m_ccbLop.DisplayMember = "MALOP";
            m_check = false;            
        }

        private void initGP()
        {
            this.BackColor = Color.FromArgb(int.Parse(DataBase.CaiDat.MAIN_BACKCOLOR));
        }

        private void m_btClose_Click(object sender, EventArgs e)
        {            
            this.Close();
        }

        private void m_btHoanTat_Click(object sender, EventArgs e)
        {
            if (m_txtMahs.Text == "")
            {
                MessageBox.Show("Mã học sinh trống", "Thông báo");
                return;
            }

            //try
            //{

            //}
            //catch (Exception)
            //{
                
            //    throw;
            //}

            hs.MAHS = int.Parse(m_txtMahs.Text);
            hs.DIACHI = m_tbDiaChi.Text;
            hs.EMAIL = m_tbEmail.Text;
            hs.GIOITINH = m_tbGioiTinh.Text;
            hs.HOTEN = m_tbHoVaTen.Text;
            pl.MALOP = m_ccbLop.SelectedValue.ToString();
            hs.NGAYSINH = m_dtpNgaysinh.Value;
            hs.TONGIAO = m_tbTonGiao.Text;
            hs.HOTENCHAC = m_tbHotencha.Text;
            hs.HOTENME = m_tbHotenme.Text;
            hs.NGHENGHIEPCHA = m_tbNghenghiepcha.Text;
            hs.NGHENGHIEPME = m_tbNghenghiepme.Text;
            hs.IMAGEE = m_imagePath;

            if (m_check)
            {
                pl.MANAMHOC = int.Parse(m_ccbManamhoc.SelectedValue.ToString());
                pl.MAKHOI = pl.MALOP.Substring(0, 2);
                hs_bus.ThemHocSinh(hs,pl);
                MessageBox.Show("Thanh cong");
            }
            else if (hs_bus.UpdateHocSinh(hs, pl)&&m_check ==false)
            {
                MessageBox.Show("Thanh cong");
                this.Close();
            }
            else MessageBox.Show("That bai");
        }

        private bool CheckEmail(string input)
        {
            string strRegex = @"\w*@\w+\w*\.\w\w";

            Regex rg = new Regex(strRegex);

            if (rg.IsMatch(input))
                return true;

            return false;
        }

        private void m_tbEmail_TextChanged(object sender, EventArgs e)
        {
            if (!CheckEmail(m_tbEmail.Text))
            {
                m_tbEmail.ForeColor = Color.Red;
            }
            else
            {
                m_tbEmail.ForeColor = Color.Black;
            }
        }

        private void m_btminimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void m_btnChonhinh_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_imagePath = openFileDialog1.FileName.ToString();
                Bitmap image = new Bitmap(m_imagePath);
                m_ptbHS.Image = (Image)image;
            }
        }

    }
}
