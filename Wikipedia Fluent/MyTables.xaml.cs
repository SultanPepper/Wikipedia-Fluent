using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Telerik.UI.Xaml.Controls.Grid;
using Telerik.Core;
using Telerik.Data;
using Telerik.UI.Xaml;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wikipedia_Fluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyTables : Page
    {
        public MyTables()
        {
            this.InitializeComponent();

            string[] strings = { "a", "b", "c" };
            string[] string2 = { "d", "e", "f" };

            RadDataGrid telerikgrid = new RadDataGrid();
            DataGridTextColumn column = new DataGridTextColumn();
            telerikgrid.AutoGenerateColumns = false;
            column.PropertyName = "1";
            column.Header = "COLUMN 1";
            





            Textblock1.Text = "\t\u2022 This is my string";


            //Extract out tables
            //pattern = @"\n{\|(.|\n)+?\n\|}";

            //Get the table caption
            //pattern = @"\|\+.+";

            //Get the headers
            //pattern = @"(?<=(^!|\n!|!!)).*(?=(\n!||!!|\|\-))"

            //Get individual rows
            //pattern = @"(?<=\|-.*\n)(\n|.)*?(?=\n(\|-|\|}))";

            // → Then parse into columns
            //pattern = @"(?<=(\n\||\|\|))[^(\-|\+)].*?(?=(\|\||\n\|))";

            string content1 = @"{| class=""wikitable sortable"" style=""margin:auto;""
|+Average temperatures and precipitation for selected communities in California
|-
!Location
! August<br>(°F)
! August<br>(°C)
! January<br>(°F)
! January<br>(°C)
! Annual<br>Precipitation<br>(mm/in)
|-
|[[Los Angeles]] || 83/64 || 29/18 || 66/48 || 20/8 || 377/15
|-
|[[Los Angeles International Airport|LAX/LA Beaches]] || 75/64 || 23/18 || 65/49 || 18/9 || 326/13
|-
|[[San Diego, California|San Diego]] || 76/67 || 24/19 || 65/49 || 18/9 || 262/10
|-
|[[San Jose, California|San Jose]] || 82/58 || 27/14 || 58/42 || 14/5 || 401/16
|-
|[[San Francisco, California|San Francisco]] || 67/54 || 20/12 || 56/46 || 14/8 || 538/21
|-
|[[Fresno, California|Fresno]] || 97/66 || 34/19 || 55/38 || 12/3 || 292/11
|-
|[[Sacramento, California|Sacramento]] || 91/58 || 33/14 || 54/39 || 12/3 || 469/18
|-
|[[Oakland, California|Oakland]] || 73/58 || 23/14 || 58/44 || 14/7 || 588/23
|-
|[[Bakersfield, California|Bakersfield]] || 96/69 || 36/21 || 56/39 || 13/3 || 165/7
|-
|[[Riverside, California|Riverside]] || 94/60 || 35/18 || 67/39  || 19/4 || 260/10
|-
|[[Eureka, California|Eureka]] || 62/53 || 16/11 || 54/41 || 12/5 || 960/38
|-
|[[Death Valley, California|Death Valley]] || 113/84 || 45/29 || 64/37 || 18/3 ||&nbsp; 53/2
|-
|[[Mammoth Lakes, California|Mammoth Lakes]] || 77/45 || 25/7 || 40/15 || 4/ -9 || 583/23
|}
            ";

            string content2 = @"{| class=""wikitable sortable"" style=""margin:auto;""
|+Average daily maximum and minimum temperatures for selected cities in Texas
|-
!Location
!August (°F)
!August (°C)
!January (°F)
!January (°C)
|-
|[[Houston, Texas|Houston]] || 94/75 || 34/24 || 63/42 || 17/6
|-
|[[San Antonio, Texas|San Antonio]] || 96/74 || 35/23 || 63/40 || 17/5
|-
|[[Dallas, Texas|Dallas]] || 96/77 || 36/25 || 57/37 || 16/3
|-
|[[Austin, Texas|Austin]] || 97/74 || 36/23 || 61/45 || 16/5
|-
|[[El Paso, Texas|El Paso]] || 92/67 || 33/21 || 57/32 || 14/0
|-
|[[Laredo, Texas|Laredo]] || 100/77 || 37/25 || 67/46 || 19/7
|-
|[[Amarillo, Texas|Amarillo]] || 89/64 || 32/18 || 50/23 || 10/–4
|-
|[[Brownsville, Texas|Brownsville]] || 94/76 || 34/24 || 70/51 || 21/11
|}
";

            ScrollViewer scrollviewer = new ScrollViewer();
            scrollviewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            Grid grid = new Grid();

            //Add newly created grid as a child
            this.mygrid.Children.Add(scrollviewer);
            scrollviewer.Content = grid;
            grid.ColumnSpacing = 15;
            grid.RowSpacing = 5;

            grid.Margin = new Thickness(20, 20, 20, 20);
            //Extract the ENTIRE table
            string input = content2;
            string pattern = @"{\|.*(.|\n)*?\|}";
            Regex regex = new Regex(pattern);
            MatchCollection EntireTables = Regex.Matches(input, pattern);

            //Add each column result to that row
            pattern = @"(?<=(^\||\n\||\|\|))[^(\-|\+)].*?(?=(\|\||\n\||$))";
            regex = new Regex(pattern);
            MatchCollection columns_count = Regex.Matches(EntireTables[0].Value, pattern);
            int column_counted = columns_count.Count;




            for (int i = 0; i < column_counted; i++)
            {
                //Create the new column
                ColumnDefinition coldef = new ColumnDefinition();
                //coldef.Width = new GridLength(1, GridUnitType.Star);
                coldef.Width = new GridLength(0, GridUnitType.Auto);
                grid.ColumnDefinitions.Add(coldef);
            }

            //Get list of entire tables
            List<string> EntireTables_list = new List<string>();
            foreach (Match match_entiretable in EntireTables)
            {
                EntireTables_list.Add(match_entiretable.Value);
            }






            //Parse out the tables


            foreach (string Table in EntireTables_list)
            {
                int current_row = 0;

                //Get each row
                pattern = @"(?<=(\|-.*\n))(\n|.)*?(?=\n(\|-|\|}))";
                regex = new Regex(pattern);
                MatchCollection rows = Regex.Matches(Table, pattern);

                pattern = @"(?<=(^!|\n!|!!)).*?(?=(!!|\n!|\n\|))";
                regex = new Regex(pattern);
                MatchCollection headers = Regex.Matches(Table, pattern);
                Match m_header = regex.Match(Table);
                int current_column = 0;
                if (m_header.Success)
                {

                    RowDefinition rowdef = new RowDefinition();
                    rowdef.Height = new GridLength(0, GridUnitType.Auto);
                    grid.RowDefinitions.Add(rowdef);

                    foreach (Match header in headers)
                    {

                        TextBlock textblock = new TextBlock();
                        textblock.Text = header.Value;
                        textblock.FontSize = 16;
                        textblock.FontWeight = FontWeights.Bold;
                        textblock.HorizontalAlignment = HorizontalAlignment.Center;
                        textblock.VerticalAlignment = VerticalAlignment.Center;

                        grid.Children.Add(textblock);

                        Grid.SetColumn(textblock, current_column);
                        Grid.SetRow(textblock, current_row);
                        current_column++;

                    }

                    current_row++;
                }

                foreach (Match m_row in rows)
                {
                    current_column = 0;

                    //Add a new row
                    RowDefinition rowdef = new RowDefinition();
                    rowdef.Height = new GridLength(0, GridUnitType.Auto);
                    grid.RowDefinitions.Add(rowdef);

                    //Add each column result to that row
                    pattern = @"(?<=(^\||\n\||\|\|))[^(\-|\+)].*?(?=(\|\||\n\||$))";
                    regex = new Regex(pattern);
                    MatchCollection columns = Regex.Matches(m_row.Value, pattern);



                    foreach (Match m_col in columns)
                    {


                        //Content we will be adding too
                        TextBlock textblock = new TextBlock();
                        textblock.Text = m_col.Value;
                        textblock.FontSize = 14;
                        textblock.FontWeight = FontWeights.SemiLight;
                        textblock.HorizontalAlignment = HorizontalAlignment.Left;
                        textblock.VerticalAlignment = VerticalAlignment.Center;

                        //Add block to grid
                        grid.Children.Add(textblock);

                        //Set the row
                        Grid.SetColumn(textblock, current_column);
                        Grid.SetRow(textblock, current_row);

                        current_column++;
                    }
                    current_row++;
                }
            }



            RichTextBlock rtb = new RichTextBlock();
            rtb.Width = 500;
            rtb.Height = 500;

            StringBuilder strTable = new StringBuilder();
            strTable.Append(@"{rtf1 ");

            //Creating 5 rows with 4 columns
            for(int i = 0; i < 5; i++)
            {
                strTable.Append(@"\trowd");

                for (int j = 1; j < 5; j++)
                {
                    int width = j * 1000;
                    strTable.Append(@"\cellx");
                    strTable.Append(width.ToString());
                }

                strTable.Append(@"\intbl \cell \row");
            }

            strTable.Append(@"\pard");

            strTable.Append(@"}");
         


            //ColumnDefinition c1 = new ColumnDefinition();
            //ColumnDefinition c2 = new ColumnDefinition();
            //ColumnDefinition c3 = new ColumnDefinition();
            //c1.Width = new GridLength(1, GridUnitType.Star);
            //c2.Width = new GridLength(1, GridUnitType.Star);
            //c3.Width = new GridLength(1, GridUnitType.Star);
            //grid.ColumnDefinitions.Add(c1);
            //grid.ColumnDefinitions.Add(c2);
            //grid.ColumnDefinitions.Add(c3);

            //RowDefinition r1 = new RowDefinition();
            //r1.Height = new GridLength(0, GridUnitType.Auto);
            //grid.RowDefinitions.Add(r1);

            //TextBlock textblk1 = new TextBlock();
            //textblk1.Text = "Textblk1A";
            //TextBlock textblk0 = new TextBlock();
            //textblk1.Text = "Textblk0";
            //TextBlock textblk2 = new TextBlock();
            //textblk2.Text = "Textblk2B";
            //TextBlock textblk3 = new TextBlock();
            //textblk3.Text = "Textblk3C";

            ////rowstackpanel.Children.Add(textblk1);

            //grid.Children.Add(textblk1);
            //grid.Children.Add(textblk2);
            //grid.Children.Add(textblk3);

            //Grid.SetColumn(textblk0, 0);
            //Grid.SetRow(textblk0, 0);
            //Grid.SetColumn(textblk2, 1);
            //Grid.SetRow(textblk2, 0);
            //Grid.SetColumn(textblk3, 2);
            //Grid.SetRow(textblk3, 0);
            
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
   



            //Extract out tables
            //pattern = @"\n{\|(.|\n)+?\n\|}";

            //Get the table caption
            //pattern = @"\|\+.+";

            //Get the headers
            //pattern = @"(?<=(^!|\n!|!!)).*(?=(\n!||!!|\|\-))"

            //Get individual rows
            //pattern = @"(?<=\|-.*\n)(\n|.)*?(?=\n(\|-|\|}))";

            // → Then parse into columns
            //pattern = @"(?<=(\n\||\|\|))[^(\-|\+)].*?(?=(\|\||\n\|))";

        }
    }
}
