using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.International.Formatters;
using System.Data.SQLite;
using System.Drawing.Printing;

namespace printpay2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
		
		private static Regex numbers = new Regex(@"^\d+(\.\d*)?$");
        //private static Regex upNum = new Regex(@"((?<man>.{1})万)?((?<sen>.{1})仟)?((?<hyk>.{1})佰)?((?<ju>.{1})拾)?((?<gen>.{1})元)?((?<kaku>.{1})角)?((?<cent>.{1})分)?$");
        //private static Regex overTenTh = new Regex(@"^((?<oku>.{1})亿)?((?<senman>.{1})仟)?((?<hykman>.{1})佰)?((?<juman>.{1})拾)?");
        private static Regex regOku = new Regex(@"(?<oku>.{1})亿");
        private static Regex regSenman = new Regex(@"(?<senman>[^亿仟佰拾万]{1})仟(?=.*万)");
        private static Regex regHykman = new Regex(@"(?<hykman>[^亿仟佰拾万]{1})佰(?=.*万)");
        private static Regex regJuman = new Regex(@"(?<juman>[^亿仟佰拾万]{1})拾(?=.*万)");
        private static Regex regMan = new Regex(@"(?<man>[^亿仟佰拾万]{1})万");
        private static Regex regSen = new Regex(@"(?<sen>[^亿仟佰拾万]{1})仟(?!.*万)");
        private static Regex regHyk = new Regex(@"(?<hyk>[^亿仟佰拾万]{1})佰(?!.*万)");
        private static Regex regJu = new Regex(@"(?<ju>[^亿仟佰拾万]{1})拾(?!.*万)");
        private static Regex regGen = new Regex(@"(?<gen>[^亿仟佰拾万]{1})元");
        private static Regex regOne = new Regex(@"(?<kaku>.{1})角(?<cent>.{1})分");
        private double unionTotal;
        private double centerTotal;
        private static List<string> units = new List<string> { "cent", "kaku", "gen", "ju", "hyk", "sen", "man", "juman", "hykman", "senman", "oku" };
        SQLiteConnection conn;

        private void Form1_Load(object sender, EventArgs e)
        {
            var today = DateTime.Today;
            textBox_year.Text = today.Year.ToString();
            textBox_month.Text = today.Month.ToString();
            textBox_day.Text = today.Day.ToString();
            conn = new SQLiteConnection("Data Source=" + @"user.db;Version=3;");
            conn.Open();
        }

        private void UnionAmount_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = unionAmount.Text != "" ? double.Parse(unionAmount.Text) : 0;
                    double num2 = unionAmount2.Text != "" ? double.Parse(unionAmount2.Text) : 0;
                    double num3 = unionAmount3.Text != "" ? double.Parse(unionAmount3.Text) : 0;
                    double num4 = unionAmount4.Text != "" ? double.Parse(unionAmount4.Text) : 0;
                    double num5 = unionAmount5.Text != "" ? double.Parse(unionAmount5.Text) : 0;
                    unionTotal = num1 + num2 + num3 + num4 + num5;
                    untotal.Text = string.Format("{0:N}", unionTotal);
                    AddUnionSum(CnCash(unionTotal));
                    AddUnionSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }

        }

        private void AddUnionSum(string amount)
        {
            Debug.WriteLine(amount);
            GroupCollection gc = regOne.Match(amount).Groups;
            unCent.Text = gc["cent"].Value;
            unKaku.Text = gc["kaku"].Value;
            unGen.Text = regGen.Match(amount).Groups["gen"].Value;
            unJu.Text = regJu.Match(amount).Groups["ju"].Value;
            unHyk.Text = regHyk.Match(amount).Groups["hyk"].Value;
            unSen.Text = regSen.Match(amount).Groups["sen"].Value;
            unMan.Text = regMan.Match(amount).Groups["man"].Value;
            unJuman.Text = regJuman.Match(amount).Groups["juman"].Value;
            unHykman.Text = regHykman.Match(amount).Groups["hykman"].Value;
            unSenman.Text = regSenman.Match(amount).Groups["senman"].Value;
            unOku.Text = regOku.Match(amount).Groups["oku"].Value;

        }

        private void AddCenterSum(string amount)
        {
            Debug.WriteLine(amount);
            GroupCollection gc = regOne.Match(amount).Groups;
            ctCent.Text = gc["cent"].Value;
            ctKaku.Text = gc["kaku"].Value;
            ctGen.Text = regGen.Match(amount).Groups["gen"].Value;
            ctJu.Text = regJu.Match(amount).Groups["ju"].Value;
            ctHyk.Text = regHyk.Match(amount).Groups["hyk"].Value;
            ctSen.Text = regSen.Match(amount).Groups["sen"].Value;
            ctMan.Text = regMan.Match(amount).Groups["man"].Value;
            ctJuman.Text = regJuman.Match(amount).Groups["juman"].Value;
            ctHykman.Text = regHykman.Match(amount).Groups["hykman"].Value;
            ctSenman.Text = regSenman.Match(amount).Groups["senman"].Value;
            ctOku.Text = regOku.Match(amount).Groups["oku"].Value;
        }

        private void AddUnionSlash()
        {
            List<bool> blank = new List<bool>();
            if (unOku.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (unSenman.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (unHykman.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (unJuman.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (unMan.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (unSen.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (unHyk.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (unJu.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (unGen.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            bool firstmet = true;
            for (int i = 0; i < blank.Count; i++)
            {
                
                if (!blank[i] && i != 0)
                {
                    if (firstmet)
                    {
                        FillwithUnion(i - 1, "ⓧ");
                        firstmet = false;
                    }
                }
                else
                {
                    if (!firstmet)
                    {
                        FillwithUnion(i, "零");
                    }
                }
                if (!blank[0])
                {
                    firstmet = false;
                }
            }
        }

        private void FillwithUnion(int i, string content)
        {
            switch (i)
            {
                case 0:
                    unOku.Text = content;
                    break;
                case 1:
                    unSenman.Text = content;
                    break;
                case 2:
                    unHykman.Text = content;
                    break;
                case 3:
                    unJuman.Text = content;
                    break;
                case 4:
                    unMan.Text = content;
                    break;
                case 5:
                    unSen.Text = content;
                    break;
                case 6:
                    unHyk.Text = content;
                    break;
                case 7:
                    unJu.Text = content;
                    break;
                case 8:
                    unGen.Text = content;
                    break;
                default:
                    break;
            }
        }

        private void AddCenterSlash()
        {
            List<bool> blank = new List<bool>();
            if (ctOku.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (ctSenman.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (ctHykman.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (ctJuman.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (ctMan.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (ctSen.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (ctHyk.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (ctJu.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            if (ctGen.Text == "")
            {
                blank.Add(true);
            }
            else
            {
                blank.Add(false);
            }
            bool firstmet = true;
            for (int i = 0; i < blank.Count; i++)
            {
                if (!blank[i] && i != 0)
                {
                    if (firstmet)
                    {
                        FillwithCenter(i - 1, "ⓧ");
                        firstmet = false;
                    }
                }
                else
                {
                    if (!firstmet)
                    {
                        FillwithCenter(i, "零");
                    }
                }
                if (!blank[0])
                {
                    firstmet = false;
                }
            }
        }

        private void FillwithCenter(int i, string content)
        {
            switch (i)
            {
                case 0:
                    ctOku.Text = content;
                    break;
                case 1:
                    ctSenman.Text = content;
                    break;
                case 2:
                    ctHykman.Text = content;
                    break;
                case 3:
                    ctJuman.Text = content;
                    break;
                case 4:
                    ctMan.Text = content;
                    break;
                case 5:
                    ctSen.Text = content;
                    break;
                case 6:
                    ctHyk.Text = content;
                    break;
                case 7:
                    ctJu.Text = content;
                    break;
                case 8:
                    ctGen.Text = content;
                    break;
                default:
                    break;
            }
        }

        private string CnCash(double sum)
        {
            if (sum > 0 && sum < 1e9)
            {
                double round = Math.Round(sum, 2) + 0.0001;
                int floor = Convert.ToInt32(Math.Floor(round));
                string after = CnUpper(round).Split(new string[] { "点" }, StringSplitOptions.RemoveEmptyEntries)[1];
                Regex ten = new Regex(@"(?<=[亿仟佰拾万])拾");
                string cnupper = CnUpper(floor);
                cnupper = ten.Replace(cnupper, "壹拾");
                if (cnupper.StartsWith("拾"))
                {
                    return "壹" + cnupper + "元" + after[0] + "角" + after[1] + "分";
                }
                else
                {
                    return cnupper + "元" + after[0] + "角" + after[1] + "分";
                }
            }
            else
            {
                return "";
            }
        }

        private string CnUpper(object num)
        {
            return InternationalNumericFormatter.FormatWithCulture("L", num, null, new System.Globalization.CultureInfo("zh-CHS"));
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void unionAmount2_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = unionAmount.Text != "" ? double.Parse(unionAmount.Text) : 0;
                    double num2 = unionAmount2.Text != "" ? double.Parse(unionAmount2.Text) : 0;
                    double num3 = unionAmount3.Text != "" ? double.Parse(unionAmount3.Text) : 0;
                    double num4 = unionAmount4.Text != "" ? double.Parse(unionAmount4.Text) : 0;
                    double num5 = unionAmount5.Text != "" ? double.Parse(unionAmount5.Text) : 0;
                    unionTotal = num1 + num2 + num3 + num4 + num5;
                    untotal.Text = string.Format("{0:N}", unionTotal);
                    AddUnionSum(CnCash(unionTotal));
                    AddUnionSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void unionAmount3_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = unionAmount.Text != "" ? double.Parse(unionAmount.Text) : 0;
                    double num2 = unionAmount2.Text != "" ? double.Parse(unionAmount2.Text) : 0;
                    double num3 = unionAmount3.Text != "" ? double.Parse(unionAmount3.Text) : 0;
                    double num4 = unionAmount4.Text != "" ? double.Parse(unionAmount4.Text) : 0;
                    double num5 = unionAmount5.Text != "" ? double.Parse(unionAmount5.Text) : 0;
                    unionTotal = num1 + num2 + num3 + num4 + num5;
                    untotal.Text = string.Format("{0:N}", unionTotal);
                    AddUnionSum(CnCash(unionTotal));
                    AddUnionSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void unionAmount4_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = unionAmount.Text != "" ? double.Parse(unionAmount.Text) : 0;
                    double num2 = unionAmount2.Text != "" ? double.Parse(unionAmount2.Text) : 0;
                    double num3 = unionAmount3.Text != "" ? double.Parse(unionAmount3.Text) : 0;
                    double num4 = unionAmount4.Text != "" ? double.Parse(unionAmount4.Text) : 0;
                    double num5 = unionAmount5.Text != "" ? double.Parse(unionAmount5.Text) : 0;
                    unionTotal = num1 + num2 + num3 + num4 + num5;
                    untotal.Text = string.Format("{0:N}", unionTotal);
                    AddUnionSum(CnCash(unionTotal));
                    AddUnionSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void unionAmount5_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = unionAmount.Text != "" ? double.Parse(unionAmount.Text) : 0;
                    double num2 = unionAmount2.Text != "" ? double.Parse(unionAmount2.Text) : 0;
                    double num3 = unionAmount3.Text != "" ? double.Parse(unionAmount3.Text) : 0;
                    double num4 = unionAmount4.Text != "" ? double.Parse(unionAmount4.Text) : 0;
                    double num5 = unionAmount5.Text != "" ? double.Parse(unionAmount5.Text) : 0;
                    unionTotal = num1 + num2 + num3 + num4 + num5;
                    untotal.Text = string.Format("{0:N}", unionTotal);
                    AddUnionSum(CnCash(unionTotal));
                    AddUnionSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void centerAmount_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = centerAmount.Text != "" ? double.Parse(centerAmount.Text) : 0;
                    double num2 = centerAmount2.Text != "" ? double.Parse(centerAmount2.Text) : 0;
                    double num3 = centerAmount3.Text != "" ? double.Parse(centerAmount3.Text) : 0;
                    double num4 = centerAmount4.Text != "" ? double.Parse(centerAmount4.Text) : 0;
                    double num5 = centerAmount5.Text != "" ? double.Parse(centerAmount5.Text) : 0;
                    centerTotal = num1 + num2 + num3 + num4 + num5;
                    cttotal.Text = string.Format("{0:N}", centerTotal);
                    AddCenterSum(CnCash(centerTotal));
                    AddCenterSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void centerAmount2_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = centerAmount.Text != "" ? double.Parse(centerAmount.Text) : 0;
                    double num2 = centerAmount2.Text != "" ? double.Parse(centerAmount2.Text) : 0;
                    double num3 = centerAmount3.Text != "" ? double.Parse(centerAmount3.Text) : 0;
                    double num4 = centerAmount4.Text != "" ? double.Parse(centerAmount4.Text) : 0;
                    double num5 = centerAmount5.Text != "" ? double.Parse(centerAmount5.Text) : 0;
                    centerTotal = num1 + num2 + num3 + num4 + num5;
                    cttotal.Text = string.Format("{0:N}", centerTotal);
                    AddCenterSum(CnCash(centerTotal));
                    AddCenterSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void centerAmount3_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = centerAmount.Text != "" ? double.Parse(centerAmount.Text) : 0;
                    double num2 = centerAmount2.Text != "" ? double.Parse(centerAmount2.Text) : 0;
                    double num3 = centerAmount3.Text != "" ? double.Parse(centerAmount3.Text) : 0;
                    double num4 = centerAmount4.Text != "" ? double.Parse(centerAmount4.Text) : 0;
                    double num5 = centerAmount5.Text != "" ? double.Parse(centerAmount5.Text) : 0;
                    centerTotal = num1 + num2 + num3 + num4 + num5;
                    cttotal.Text = string.Format("{0:N}", centerTotal);
                    AddCenterSum(CnCash(centerTotal));
                    AddCenterSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void centerAmount4_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = centerAmount.Text != "" ? double.Parse(centerAmount.Text) : 0;
                    double num2 = centerAmount2.Text != "" ? double.Parse(centerAmount2.Text) : 0;
                    double num3 = centerAmount3.Text != "" ? double.Parse(centerAmount3.Text) : 0;
                    double num4 = centerAmount4.Text != "" ? double.Parse(centerAmount4.Text) : 0;
                    double num5 = centerAmount5.Text != "" ? double.Parse(centerAmount5.Text) : 0;
                    centerTotal = num1 + num2 + num3 + num4 + num5;
                    cttotal.Text = string.Format("{0:N}", centerTotal);
                    AddCenterSum(CnCash(centerTotal));
                    AddCenterSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void centerAmount5_TextChanged(object sender, EventArgs e)
        {
            string input = ((TextBox)sender).Text;
            if (input != "")
            {
                if (numbers.IsMatch(input))
                {
                    double num1 = centerAmount.Text != "" ? double.Parse(centerAmount.Text) : 0;
                    double num2 = centerAmount2.Text != "" ? double.Parse(centerAmount2.Text) : 0;
                    double num3 = centerAmount3.Text != "" ? double.Parse(centerAmount3.Text) : 0;
                    double num4 = centerAmount4.Text != "" ? double.Parse(centerAmount4.Text) : 0;
                    double num5 = centerAmount5.Text != "" ? double.Parse(centerAmount5.Text) : 0;
                    centerTotal = num1 + num2 + num3 + num4 + num5;
                    cttotal.Text = string.Format("{0:N}", centerTotal);
                    AddCenterSum(CnCash(centerTotal));
                    AddCenterSlash();
                }
                else
                {
                    ((TextBox)sender).Text = "";
                    MessageBox.Show("必须输入数字");
                }
            }
        }

        private void payee_TextChanged(object sender, EventArgs e)
        {
            string name = ((TextBox)sender).Text;
            string sql = string.Format("SELECT * FROM payee WHERE payee = '{0}'", name);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return;
            }
            
            while (reader.Read())
            {
                bank.Items.Clear();
                account.Items.Clear();
                bank.Text = reader[2].ToString();
                account.Text = reader[3].ToString();
                bank.Items.Add(reader[2]);
                account.Items.Add(reader[3]);
            }
            
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
        }

        private void payee2_TextChanged(object sender, EventArgs e)
        {
            string name = ((TextBox)sender).Text;
            string sql = string.Format("SELECT * FROM payee WHERE payee = '{0}'", name);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return;
            }

            while (reader.Read())
            {
                bank2.Items.Clear();
                account2.Items.Clear();
                bank2.Text = reader[2].ToString();
                account2.Text = reader[3].ToString();
                bank2.Items.Add(reader[2]);
                account2.Items.Add(reader[3]);
            }
        }

        private void payee3_TextChanged(object sender, EventArgs e)
        {
            string name = ((TextBox)sender).Text;
            string sql = string.Format("SELECT * FROM payee WHERE payee = '{0}'", name);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return;
            }

            while (reader.Read())
            {
                bank3.Items.Clear();
                account3.Items.Clear();
                bank3.Text = reader[2].ToString();
                account3.Text = reader[3].ToString();
                bank3.Items.Add(reader[2]);
                account3.Items.Add(reader[3]);
            }
        }

        private void payee4_TextChanged(object sender, EventArgs e)
        {
            string name = ((TextBox)sender).Text;
            string sql = string.Format("SELECT * FROM payee WHERE payee = '{0}'", name);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return;
            }

            while (reader.Read())
            {
                bank4.Items.Clear();
                account4.Items.Clear();
                bank4.Text = reader[2].ToString();
                account4.Text = reader[3].ToString();
                bank4.Items.Add(reader[2]);
                account4.Items.Add(reader[3]);
            }
        }

        private void payee5_TextChanged(object sender, EventArgs e)
        {
            string name = ((TextBox)sender).Text;
            string sql = string.Format("SELECT * FROM payee WHERE payee = '{0}'", name);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                return;
            }

            while (reader.Read())
            {
                bank5.Items.Clear();
                account5.Items.Clear();
                bank5.Text = reader[2].ToString();
                account5.Text = reader[3].ToString();
                bank5.Items.Add(reader[2]);
                account5.Items.Add(reader[3]);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font ft = new Font("宋体", 11, FontStyle.Regular);
            SolidBrush sb = new SolidBrush(Color.Black);
            string from = textBox_from.Text;
            string year = textBox_year.Text;
            string month = textBox_month.Text;
            string day = textBox_day.Text;
            string payway = payWay.Text;
            string payway2 = payWay2.Text;
            string payway3 = payWay3.Text;
            string payway4 = payWay4.Text;
            string payway5 = payWay5.Text;
            string paye = payee.Text;
            string paye2 = payee2.Text;
            string paye3 = payee3.Text;
            string paye4 = payee4.Text;
            string paye5 = payee5.Text;
            string ban = bank.Text;
            string ban2 = bank2.Text;
            string ban3 = bank3.Text;
            string ban4 = bank4.Text;
            string ban5 = bank5.Text;
            string acc = account.Text;
            string acc2 = account2.Text;
            string acc3 = account3.Text;
            string acc4 = account4.Text;
            string acc5 = account5.Text;
            string ua = unionAmount.Text != "" ? string.Format("{0:N}", double.Parse(unionAmount.Text)) : "";
            string ua2 = unionAmount2.Text != "" ? string.Format("{0:N}", double.Parse(unionAmount2.Text)) : "";
            string ua3 = unionAmount3.Text != "" ? string.Format("{0:N}", double.Parse(unionAmount3.Text)) : "";
            string ua4 = unionAmount4.Text != "" ? string.Format("{0:N}", double.Parse(unionAmount4.Text)) : "";
            string ua5 = unionAmount5.Text != "" ? string.Format("{0:N}", double.Parse(unionAmount5.Text)) : "";
            string ca = centerAmount.Text != "" ? string.Format("{0:N}", double.Parse(centerAmount.Text)) : "";
            string ca2 = centerAmount2.Text != "" ? string.Format("{0:N}", double.Parse(centerAmount2.Text)) : "";
            string ca3 = centerAmount3.Text != "" ? string.Format("{0:N}", double.Parse(centerAmount3.Text)) : "";
            string ca4 = centerAmount4.Text != "" ? string.Format("{0:N}", double.Parse(centerAmount4.Text)) : "";
            string ca5 = centerAmount5.Text != "" ? string.Format("{0:N}", double.Parse(centerAmount5.Text)) : "";
            string unt = untotal.Text;
            string ctt = cttotal.Text;
            e.Graphics.DrawString(from, ft, sb, new PointF(300F, 15F));
            e.Graphics.DrawString(year, ft, sb, new PointF(415F, 53F));
            e.Graphics.DrawString(month, ft, sb, new PointF(480F, 53F));
            e.Graphics.DrawString(day, ft, sb, new PointF(520F, 53F));
            e.Graphics.DrawString(payway, ft, sb, new PointF(47F, 135F));
            e.Graphics.DrawString(payway2, ft, sb, new PointF(47F, 165F));
            e.Graphics.DrawString(payway3, ft, sb, new PointF(47F, 195F));
            e.Graphics.DrawString(payway4, ft, sb, new PointF(47F, 225F));
            e.Graphics.DrawString(payway5, ft, sb, new PointF(47F, 255F));
            e.Graphics.DrawString(paye, ft, sb, new PointF(170F, 135F));
            e.Graphics.DrawString(paye2, ft, sb, new PointF(170F, 165F));
            e.Graphics.DrawString(paye3, ft, sb, new PointF(170F, 195F));
            e.Graphics.DrawString(paye4, ft, sb, new PointF(170F, 225F));
            e.Graphics.DrawString(paye5, ft, sb, new PointF(170F, 255F));
            e.Graphics.DrawString(ban, ft, sb, new PointF(360F, 135F));
            e.Graphics.DrawString(ban2, ft, sb, new PointF(360F, 165F));
            e.Graphics.DrawString(ban3, ft, sb, new PointF(360F, 195F));
            e.Graphics.DrawString(ban4, ft, sb, new PointF(360F, 225F));
            e.Graphics.DrawString(ban5, ft, sb, new PointF(360F, 255F));
            e.Graphics.DrawString(acc, ft, sb, new PointF(480F, 135F));
            e.Graphics.DrawString(acc2, ft, sb, new PointF(480F, 165F));
            e.Graphics.DrawString(acc3, ft, sb, new PointF(480F, 195F));
            e.Graphics.DrawString(acc4, ft, sb, new PointF(480F, 225F));
            e.Graphics.DrawString(acc5, ft, sb, new PointF(480F, 255F));
            e.Graphics.DrawString(ua, ft, sb, new PointF(620F, 135F));
            e.Graphics.DrawString(ua2, ft, sb, new PointF(620F, 165F));
            e.Graphics.DrawString(ua3, ft, sb, new PointF(620F, 195F));
            e.Graphics.DrawString(ua4, ft, sb, new PointF(620F, 225F));
            e.Graphics.DrawString(ua5, ft, sb, new PointF(620F, 255F));
            e.Graphics.DrawString(ca, ft, sb, new PointF(710F, 135F));
            e.Graphics.DrawString(ca2, ft, sb, new PointF(710F, 165F));
            e.Graphics.DrawString(ca3, ft, sb, new PointF(710F, 195F));
            e.Graphics.DrawString(ca4, ft, sb, new PointF(710F, 225F));
            e.Graphics.DrawString(ca5, ft, sb, new PointF(710F, 255F));
            e.Graphics.DrawString(unt, ft, sb, new PointF(620F, 285F));
            e.Graphics.DrawString(ctt, ft, sb, new PointF(710F, 285F));
            e.Graphics.DrawString(unCent.Text, ft, sb, new PointF(575F, 285F));
            e.Graphics.DrawString(ctCent.Text, ft, sb, new PointF(575F, 315F));
            e.Graphics.DrawString(unKaku.Text, ft, sb, new PointF(538F, 285F));
            e.Graphics.DrawString(ctKaku.Text, ft, sb, new PointF(538F, 315F));
            e.Graphics.DrawString(unGen.Text, ft, sb, new PointF(501F, 285F));
            e.Graphics.DrawString(ctGen.Text, ft, sb, new PointF(501F, 315F));
            e.Graphics.DrawString(unJu.Text, ft, sb, new PointF(464F, 285F));
            e.Graphics.DrawString(ctJu.Text, ft, sb, new PointF(464F, 315F));
            e.Graphics.DrawString(unHyk.Text, ft, sb, new PointF(427F, 285F));
            e.Graphics.DrawString(ctHyk.Text, ft, sb, new PointF(427F, 315F));
            e.Graphics.DrawString(unSen.Text, ft, sb, new PointF(390F, 285F));
            e.Graphics.DrawString(ctSen.Text, ft, sb, new PointF(390F, 315F));
            e.Graphics.DrawString(unMan.Text, ft, sb, new PointF(353F, 285F));
            e.Graphics.DrawString(ctMan.Text, ft, sb, new PointF(353F, 315F));
            e.Graphics.DrawString(unJuman.Text, ft, sb, new PointF(316F, 285F));
            e.Graphics.DrawString(ctJuman.Text, ft, sb, new PointF(316F, 315F));
            e.Graphics.DrawString(unHykman.Text, ft, sb, new PointF(279F, 285F));
            e.Graphics.DrawString(ctHykman.Text, ft, sb, new PointF(279F, 315F));
            e.Graphics.DrawString(unSenman.Text, ft, sb, new PointF(242F, 285F));
            e.Graphics.DrawString(ctSenman.Text, ft, sb, new PointF(242F, 315F));
            e.Graphics.DrawString(unOku.Text, ft, sb, new PointF(205F, 285F));
            e.Graphics.DrawString(ctOku.Text, ft, sb, new PointF(205F, 315F));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string selsql = "SELECT * FROM payee WHERE payee = '{0}' AND bank = '{1}' AND account = '{2}'";
            if (payee.Text != "")
            {
                SQLiteCommand command = new SQLiteCommand(string.Format(selsql, new string[] { payee.Text, bank.Text, account.Text }), conn);
                SQLiteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    string insql = string.Format("INSERT INTO payee (payee, bank, account) VALUES ('{0}', '{1}', '{2}')", new string[] { payee.Text, bank.Text, account.Text });
                    SQLiteCommand comm = new SQLiteCommand(insql, conn);
                    comm.ExecuteNonQuery();
                }
            }
            if (payee2.Text != "")
            {
                SQLiteCommand command = new SQLiteCommand(string.Format(selsql, new string[] { payee2.Text, bank2.Text, account2.Text }), conn);
                SQLiteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    string insql = string.Format("INSERT INTO payee (payee, bank, account) VALUES ('{0}', '{1}', '{2}')", new string[] { payee2.Text, bank2.Text, account2.Text });
                    SQLiteCommand comm = new SQLiteCommand(insql, conn);
                    comm.ExecuteNonQuery();
                }
            }
            if (payee3.Text != "")
            {
                SQLiteCommand command = new SQLiteCommand(string.Format(selsql, new string[] { payee3.Text, bank3.Text, account3.Text }), conn);
                SQLiteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    string insql = string.Format("INSERT INTO payee (payee, bank, account) VALUES ('{0}', '{1}', '{2}')", new string[] { payee.Text, bank.Text, account.Text });
                    SQLiteCommand comm = new SQLiteCommand(insql, conn);
                    comm.ExecuteNonQuery();
                }
            }
            if (payee4.Text != "")
            {
                SQLiteCommand command = new SQLiteCommand(string.Format(selsql, new string[] { payee4.Text, bank4.Text, account4.Text }), conn);
                SQLiteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    string insql = string.Format("INSERT INTO payee (payee, bank, account) VALUES ('{0}', '{1}', '{2}')", new string[] { payee4.Text, bank4.Text, account4.Text });
                    SQLiteCommand comm = new SQLiteCommand(insql, conn);
                    comm.ExecuteNonQuery();
                }
            }
            if (payee5.Text != "")
            {
                SQLiteCommand command = new SQLiteCommand(string.Format(selsql, new string[] { payee5.Text, bank5.Text, account5.Text }), conn);
                SQLiteDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    string insql = string.Format("INSERT INTO payee (payee, bank, account) VALUES ('{0}', '{1}', '{2}')", new string[] { payee5.Text, bank5.Text, account5.Text });
                    SQLiteCommand comm = new SQLiteCommand(insql, conn);
                    comm.ExecuteNonQuery();
                }
            }
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
