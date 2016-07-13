using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private DicomDic myDicomDic = new DicomDic();
        private DicomService myService = new DicomService();
        private Dictionary<string, string> myDict = new Dictionary<string, string>();
        private Dictionary<string, byte[]> myMap = new Dictionary<string, byte[]>();
        private int rows, columns;
        public int wValue, cValue;

        private byte[] pixelData;

        public Form1(Dictionary<string, byte[]> dicomMap)
        {
            InitializeComponent();
            //Set dictionary for tags
            myDict = myDicomDic.getDic();

            //Set 'map' for the data of the dicom file
            myMap = dicomMap;

            //Set necessary informations
            rows = BitConverter.ToInt16(myMap["00280010"], 0);
            columns = BitConverter.ToInt16(myMap["00280011"], 0);
            pixelData = myMap["7FE00010"];

            //Get all keys and theyr associated values
            foreach (string id in dicomMap.Keys)
            {
                ListViewItem item;
                byte[] value = myMap[id];

                //Check if the given id is key of the given dictonary then use the found string, otherwise use the raw id
                if (myDict.ContainsKey(id))
                {
                    item = new ListViewItem(myDict[id]);
                } else
                {
                    item = new ListViewItem(id);
                }

                item.SubItems.Add(Encoding.Default.GetString(value));
                listView1.Items.Add(item);
            }

            pictureBox1.Image = myService.createPicture(rows, columns, pixelData, trackBarC.Value, trackBarW.Value);

        }


        //Events for the form
        private void trackBarC_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = myService.createPicture(rows, columns, pixelData, trackBarC.Value, trackBarW.Value);
        }

        private void trackBarW_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = myService.createPicture(rows, columns, pixelData, trackBarC.Value, trackBarW.Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
