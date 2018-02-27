using System;
using System.Drawing; //Needed for Point to position form items and for .Font
using System.Data; //needed for IDataRecord
using System.Data.SqlClient;
using System.Windows.Forms;


class Program
{
	[STAThread] //https://blogs.msdn.microsoft.com/jfoscoding/2005/04/07/why-is-stathread-required/
	static void Main() 
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(new oneForm()); //constructor of the oneForm class		
	}
}

public class SQLinfo
{
	static string sconnect = "Data Source=VIDEOCONFERENCE\\sqlexpress;Initial Catalog=master;Integrated Security=True";

	
	public static string ghmdatabase(int ghpRow)
	{
	string sqlrowdata = String.Empty;	
		using (SqlConnection ghvSqlConn = new SqlConnection())
		{
			try
			{
				ghvSqlConn.ConnectionString = sconnect;
				ghvSqlConn.Open();
				SqlCommand ghvSqlCommand = new SqlCommand("SELECT * FROM dbo.gregplay WHERE id = @ghpRow;", ghvSqlConn);
				ghvSqlCommand.Parameters.AddWithValue("@ghpRow", ghpRow);
				SqlDataReader ghvReader = ghvSqlCommand.ExecuteReader();

					oneForm.ghvColsPrpty=ghvReader.FieldCount;

					while (ghvReader.Read()) //ghvReader[0] is the first column
						{
							oneForm.ghvReaderRows++;
							oneForm.ghvFieldAmount=oneForm.ghvFieldAmount+ghvReader.FieldCount;
						
							//if(oneForm.ghvReaderRows==ghpRow){								
							sqlrowdata = String.Format("{0}, {1}", ghvReader[0], ghvReader[1]);	
							//}
							
							//oneForm.sqlfield1=ghvReader.GetString(1);
							//oneForm.sqlresultstext = oneForm.sqlfield1;
						}


				ghvReader.Close();// Call Close when done reading.
				//ghvSqlConn.Close(); //not needed as connection closed by using block
			}

			catch (SqlException ghvEr)
			{
				sqlrowdata="Error: " + ghvEr.Message;
				ghvSqlConn.Close(); //needed here?
								
			}
		}
	return sqlrowdata;
	}
}

public class oneForm : Form
{
	public static string ghvTitle = "Greg App 01";
	public static int sqlchoice;
	public static string sqlresultstext;
	public static int ghvReaderRows = 0;
	public static int ghvFieldAmount = 0;
	public static string ghvlabel2text="000";	
	private static int ghvCols = 0;
	public static int ghvColsPrpty
	{
	get { return ghvCols; }
	set { ghvCols = value; }
	}
	


	void button_Click(Object sender, EventArgs e)
	{
	Application.Exit();
	}

	void textBox1_TextChanged(object sender, EventArgs e){}

	void num1_ValueChanged(object sender, EventArgs e)
	{
	sqllabel1.Text = SQLinfo.ghmdatabase((int)num1.Value);
	ghflabel2.Text = String.Format("{0}",num1.Value);
	}	
	
	//private Label ghfTitle;
	private Label sqllabel1;
	//private Label SqlLabelNumberOfRows;
	//private Button gregbutton;
	//private TextBox textBox1;
	private NumericUpDown num1;
	private Label ghflabel2;
	

	public oneForm()  //constructor for the class extending form
	{
		this.Text = ghvTitle;
		
		Label ghfTitle = new Label();
		ghfTitle.AutoSize = true;
		ghfTitle.Text = "Greg's attempt at connecting to SQL";		
		ghfTitle.Location = new System.Drawing.Point(40,30);
		this.Controls.Add(ghfTitle);

		this.sqllabel1 = new Label();
		sqllabel1.AutoSize = true;
		sqllabel1.Location = new System.Drawing.Point(40,120);
		this.Controls.Add(sqllabel1);

		Label SqlLabelNumberOfRows = new Label();
		SqlLabelNumberOfRows.AutoSize = true;
		SqlLabelNumberOfRows.Text = oneForm.ghvColsPrpty.ToString()+" columns, "+   oneForm.ghvReaderRows.ToString()+" rows and "+oneForm.ghvFieldAmount.ToString()+" fields";
		SqlLabelNumberOfRows.Location = new System.Drawing.Point(40,100);
		this.Controls.Add(SqlLabelNumberOfRows);

		Button button = new Button();
		button.Text = "Close";
		button.Location = new System.Drawing.Point(40,220);
		this.Controls.Add(button);
		button.Click += new EventHandler(button_Click);

		//textBox1
		TextBox textBox1 = new TextBox();
		textBox1.Location = new System.Drawing.Point(40,160);
		textBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
		textBox1.Name = "textBox1";
		//textBox1.Size = new System.Drawing.Size(185, 20);
		this.Controls.Add(textBox1);
		textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);

		//num1
		this.num1 = new NumericUpDown();
		this.num1.AutoSize = true;
		num1.Location = new System.Drawing.Point(40,180);
		this.num1.TabIndex = 4;
		num1.ValueChanged += new System.EventHandler(this.num1_ValueChanged);	
		this.Controls.Add(num1);
		
		this.ghflabel2 = new Label();
		//ghflabel2.Text = ghvlabel2text;
		ghflabel2.Location = new System.Drawing.Point(180,180);
		this.Controls.Add(ghflabel2);

	}

		
}
