using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using PKHeX.Core;
using PKHeX.Drawing.PokeSprite;
using PKHeX.Drawing.PokeSprite.Properties;


namespace NXWonderRecord;

public class WonderRecordLGPE : Form
{
	private bool FirstStringLoad = true;

	private readonly SaveFile sav;

	private int ClickedSlot = 0;

	private bool InitialLoad = false;

	private IDictionary<string, string> alt_dict = new Dictionary<string, string>();

	private IDictionary<string, string> alt_dex = new Dictionary<string, string>();

	private byte[] WRBlock = new byte[3200];

	private byte[] temparray = new byte[320];

	private bool LoadingTimeStamp = false;

	private bool EditingTimeStamp = false;

	private IContainer components = null;

	private Label LocationLabel;

	private NumericUpDown SlotIndex;

	private ComboBox SpeciesBox;

	private PictureBox SpeciesImageBox;

	private Label AreaSlotLabel;

	private Label SpeciesLabel;

	private NumericUpDown FormBox;

	private Label label1;

	private ComboBox EntryTypeBox;

	private NumericUpDown WCIDBox;

	private Label label2;

	internal ComboBox CardTitleRawBox;

	private Label label3;

	private TabControl WRTabs;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private TabPage tabPage3;

	internal Label Label28;

	internal ComboBox move1list;

	internal ComboBox move2list;

	internal ComboBox move3list;

	internal ComboBox move4list;

	internal Label label11;

	internal Label label10;

	internal Label Label4;

	internal ComboBox pokedextype;

	internal Label Label5;

	internal Label Label21;

	internal Label Label6;

	internal TextBox pokemonlc;

	internal TextBox otnamebox;

	internal ComboBox OTG;

	internal ComboBox languagebox;

	internal Label Label7;

	internal TextBox CardTitleRefinedBox;

	internal ComboBox itemnameplural;

	internal TextBox nitem6;

	internal TextBox nitem5;

	internal TextBox nitem4;

	internal TextBox nitem3;

	internal TextBox nitem2;

	internal TextBox nitem1;

	internal TextBox itemslc;

	internal Label Label20;

	internal Label Label18;

	internal Label Label17;

	internal Label Label16;

	internal Label Label15;

	internal Label Label14;

	internal ComboBox itembox6;

	internal ComboBox itembox5;

	internal ComboBox itembox4;

	internal ComboBox itembox3;

	internal ComboBox itembox2;

	internal ComboBox itembox1;

	internal Label Label13;

	private Button WC8_2_WR8_Button;

	private Label label34;

	internal TextBox TimestampBox;

	private Label label19;

	private GroupBox groupBox1;

	private Label label40;

	private Label label38;

	private Label label39;

	private Label label36;

	private Label label35;

	private NumericUpDown HourBox;

	private NumericUpDown YearBox;

	private NumericUpDown MonthBox;

	private NumericUpDown DayBox;

	private NumericUpDown MinBox;

	private NumericUpDown SecBox;

	private Button DateNul;

	private Button InsertWR8Button;

	private Button ExtractWR8Button;

	private Button DeleteWR8Button;

	internal TextBox RefinedSpeciesBox;

	private PictureBox pictureBox10;

	private PictureBox pictureBox9;

	private PictureBox pictureBox8;

	private PictureBox pictureBox7;

	private PictureBox pictureBox6;

	private PictureBox pictureBox5;

	private PictureBox pictureBox4;

	private PictureBox pictureBox3;

	private PictureBox pictureBox2;

	private PictureBox pictureBox1;

	private PictureBox pictureBox0;

	public WonderRecordLGPE(SaveFile sav)
	{
		this.sav = sav;
		InitializeComponent();
		InitialLoad = true;
		InitialLoading();
		if (sav is SAV7b sAV7b)
		{
			WRBlock = sAV7b.GetData(284160, 3200);
			InitialLoad = false;
			loadslots();
			((PictureBox)base.Controls["pictureBox1".ToString()]).BorderStyle = BorderStyle.Fixed3D;
			UpdateEntriesLGPE(WRBlock, 0);
		}
	}

	public void InitialLoading()
	{
		if (InitialLoad)
		{
			SlotIndex.Value = 0m;
			EntryTypeBox.SelectedIndex = 0;
			Populate_Alt_Dict();
			SpeciesBox.Items.Clear();
			SpeciesBox.DataSource = GameInfo.Strings.specieslist;
			SpeciesBox.SelectedIndex = 0;
			move1list.Items.Clear();
			move2list.Items.Clear();
			move3list.Items.Clear();
			move4list.Items.Clear();
			move1list.DataSource = GameInfo.Strings.movelist.Clone();
			move2list.DataSource = GameInfo.Strings.movelist.Clone();
			move3list.DataSource = GameInfo.Strings.movelist.Clone();
			move4list.DataSource = GameInfo.Strings.movelist.Clone();
			move1list.SelectedIndex = 0;
			move2list.SelectedIndex = 0;
			move3list.SelectedIndex = 0;
			move4list.SelectedIndex = 0;
			itembox1.DataSource = GameInfo.Strings.GetItemStrings(sav.Context, sav.Version).ToArray().Clone();
			itembox2.DataSource = GameInfo.Strings.GetItemStrings(sav.Context, sav.Version).ToArray().Clone();
			itembox3.DataSource = GameInfo.Strings.GetItemStrings(sav.Context, sav.Version).ToArray().Clone();
			itembox4.DataSource = GameInfo.Strings.GetItemStrings(sav.Context, sav.Version).ToArray().Clone();
			itembox5.DataSource = GameInfo.Strings.GetItemStrings(sav.Context, sav.Version).ToArray().Clone();
			itembox6.DataSource = GameInfo.Strings.GetItemStrings(sav.Context, sav.Version).ToArray().Clone();
			itembox1.SelectedIndex = 0;
			itembox2.SelectedIndex = 0;
			itembox3.SelectedIndex = 0;
			itembox4.SelectedIndex = 0;
			itembox5.SelectedIndex = 0;
			itembox6.SelectedIndex = 0;
			otnamebox.Text = " ";
			CardTitleRefinedBox.Text = " ";
		}
	}

	public void UpdateEntriesLGPE(byte[] TempBlock, int Index)
	{
		if (!InitialLoad)
		{
			Array.Clear(temparray, 0, temparray.Length);
			int num = 0;
			for (num = 0; num < 320; num++)
			{
				temparray[num] = TempBlock[num + Index * 320];
			}
			TimestampBox.Text = BitConverter.ToUInt32(temparray, 0).ToString();
			WCIDBox.Value = BitConverter.ToUInt16(temparray, 8);
			if (temparray[10] < 24)
			{
				CardTitleRawBox.SelectedIndex = temparray[10];
			}
			else
			{
				CardTitleRawBox.SelectedIndex = 24;
			}
			if (temparray[12] < 3)
			{
				EntryTypeBox.SelectedIndex = temparray[12];
			}
			else
			{
				EntryTypeBox.SelectedIndex = 3;
			}
			switch (EntryTypeBox.SelectedIndex)
			{
			case 0:
				WRTabs.SelectedIndex = 0;
				CardTitleRawBox.Text = "None";
				CardTitleRefinedBox.Text = "None";
				WCIDBox.Text = "0";
				SpeciesImageSelector();
				loadslots();
				break;
			case 1:
				WRLoadPokemonSWSH();
				break;
			case 2:
				WRLoadItemsSWSH();
				break;
			}
		}
	}

	private void UpdateEntriesFinally()
	{
		SpeciesImageSelector();
		loadslots();
		CardTitleRefinedBox.Text = desccall_LGPE((uint)CardTitleRawBox.SelectedIndex);
	}

	public static string ToReadableByteArray(byte[] bytes)
	{
		return string.Join(", ", bytes);
	}

	private void WRLoadPokemonSWSH()
	{
		WRTabs.SelectedIndex = 1;
		byte[] array = new byte[24];
		string text = "";
		for (int i = 0; i < 24; i++)
		{
			array[i] = temparray[288 + i];
		}
		text = Encoding.Unicode.GetString(array);
		otnamebox.Text = text;
		if (FirstStringLoad)
		{
			byte[] bytes = new byte[2];
			int length = 0;
			for (int num = 11; num >= 0; num--)
			{
				if (otnamebox.Text.Substring(num, 1) != Encoding.Unicode.GetString(bytes))
				{
					length = num + 1;
					break;
				}
			}
			otnamebox.Text = otnamebox.Text.Substring(0, length);
			FirstStringLoad = false;
		}
		if (BitConverter.ToUInt16(temparray, 268) > SpeciesBox.Items.Count)
		{
			SpeciesBox.SelectedIndex = 0;
		}
		else
		{
			SpeciesBox.SelectedIndex = BitConverter.ToUInt16(temparray, 268);
		}
		if (BitConverter.ToUInt16(temparray, 272) > move1list.Items.Count)
		{
			move1list.SelectedIndex = 0;
		}
		else
		{
			move1list.SelectedIndex = BitConverter.ToUInt16(temparray, 272);
		}
		if (BitConverter.ToUInt16(temparray, 276) > move2list.Items.Count)
		{
			move2list.SelectedIndex = 0;
		}
		else
		{
			move2list.SelectedIndex = BitConverter.ToUInt16(temparray, 276);
		}
		if (BitConverter.ToUInt16(temparray, 280) > move3list.Items.Count)
		{
			move3list.SelectedIndex = 0;
		}
		else
		{
			move3list.SelectedIndex = BitConverter.ToUInt16(temparray, 280);
		}
		if (BitConverter.ToUInt16(temparray, 284) > move4list.Items.Count)
		{
			move4list.SelectedIndex = 0;
		}
		else
		{
			move4list.SelectedIndex = BitConverter.ToUInt16(temparray, 284);
		}
		if (temparray[314] > languagebox.Items.Count)
		{
			languagebox.SelectedIndex = 0;
		}
		else
		{
			languagebox.SelectedIndex = temparray[314];
		}
		UpdateEntriesFinally();
	}

	private void WRLoadItemsSWSH()
	{
		WRTabs.SelectedIndex = 2;
		if (BitConverter.ToUInt16(temparray, 270) > itembox1.Items.Count)
		{
			itembox1.SelectedIndex = 0;
		}
		else
		{
			itembox1.SelectedIndex = BitConverter.ToUInt16(temparray, 270);
		}
		nitem1.Text = BitConverter.ToUInt16(temparray, 272).ToString();
		if (BitConverter.ToUInt16(temparray, 274) > itembox1.Items.Count)
		{
			itembox2.SelectedIndex = 0;
		}
		else
		{
			itembox2.SelectedIndex = BitConverter.ToUInt16(temparray, 274);
		}
		nitem2.Text = BitConverter.ToUInt16(temparray, 276).ToString();
		if (BitConverter.ToUInt16(temparray, 278) > itembox1.Items.Count)
		{
			itembox3.SelectedIndex = 0;
		}
		else
		{
			itembox3.SelectedIndex = BitConverter.ToUInt16(temparray, 278);
		}
		nitem3.Text = BitConverter.ToUInt16(temparray, 280).ToString();
		if (BitConverter.ToUInt16(temparray, 282) > itembox1.Items.Count)
		{
			itembox4.SelectedIndex = 0;
		}
		else
		{
			itembox4.SelectedIndex = BitConverter.ToUInt16(temparray, 282);
		}
		nitem4.Text = BitConverter.ToUInt16(temparray, 284).ToString();
		if (BitConverter.ToUInt16(temparray, 286) > itembox1.Items.Count)
		{
			itembox5.SelectedIndex = 0;
		}
		else
		{
			itembox5.SelectedIndex = BitConverter.ToUInt16(temparray, 286);
		}
		nitem5.Text = BitConverter.ToUInt16(temparray, 288).ToString();
		if (BitConverter.ToUInt16(temparray, 290) > itembox1.Items.Count)
		{
			itembox6.SelectedIndex = 0;
		}
		else
		{
			itembox6.SelectedIndex = BitConverter.ToUInt16(temparray, 290);
		}
		nitem6.Text = BitConverter.ToUInt16(temparray, 292).ToString();
		itemslc.Text = temparray[13].ToString();
		UpdateEntriesFinally();
	}

	private void SlotIndex_ValueChanged(object sender, EventArgs e)
	{
		if (!InitialLoad)
		{
			UpdateEntriesLGPE(WRBlock, (int)SlotIndex.Value);
		}
	}

	private void WonderRecord_Load(object sender, EventArgs e)
	{
		AllowDrop = false;
		base.DragEnter += WonderRecordForm_DragEnter;
		base.DragDrop += WonderRecordForm_DragDrop;
	}

	private string desccall_LGPE(uint definedvalue)
	{
		uint num = definedvalue;
		string text = "";
		string text2 = SpeciesBox.Text;
		string text3 = itembox1.Text;
		string text4 = otnamebox.Text;
		switch (num)
		{
		case 0u:
			return text2 + " Gift";
		case 1u:
			return text3 + " Gift";
		case 2u:
			return "Item Set Gift";
		case 3u:
			pokedextype.SelectedIndex = SpeciesBox.SelectedIndex;
			return pokedextype.Text + " " + text2 + " Gift";
		case 4u:
			return "Mythical Pokémon " + text2 + " Gift";
		case 5u:
			return $"{text4}'s {text2} Gift";
		case 6u:
			return "Shiny " + text2 + " Gift";
		case 7u:
			return text2 + "() Gift";
		case 8u:
			return "() Gift";
		case 9u:
			return "Hidden Ability " + text2 + " Gift";
		case 10u:
			return move1list.Text + " " + text2 + " Gift";
		case 11u:
			return " " + text2 + " with " + move2list.Text + " Gift";
		case 12u:
			return " " + text2 + " with " + move3list.Text + " Gift";
		case 13u:
			return " " + text2 + " with " + move4list.Text + " Gift";
		case 14u:
			return text2 + " & None Gift";
		case 15u:
			return "Downloadable Version Bonus";
		case 16u:
			return "Special Pack Purchase Bonus";
		case 17u:
			return "Store Purchase Bonus";
		case 18u:
			return "Strategy Guide Purchase Bonus";
		case 19u:
			return "Purchase Bonus";
		case 20u:
			return "Happy Birthday!";
		case 21u:
			return "Virtual Console Bonus";
		case 22u:
			return "Pokémon Trainer Club Gift";
		case 23u:
			return "Pokémon Global Link Gift";
		case 24u:
			return "Pokémon Bank Gift";
		default:
			return text2 + "[ID " + num + "]";
		}
	}

	private void WC8_2_WR8_Button_Click(object sender, EventArgs e)
	{
	}

	private void WC8_Load(string path)
	{
	}

	private void WC8_Pokemon(byte[] tempWC8_2_WR8, byte[] tempWC8)
	{
		ushort value = BitConverter.ToUInt16(tempWC8, 558);
		BitConverter.GetBytes(value).CopyTo(tempWC8_2_WR8, 16);
		tempWC8_2_WR8[18] = tempWC8[581];
		ushort value2 = BitConverter.ToUInt16(tempWC8, 576);
		BitConverter.GetBytes(value2).CopyTo(tempWC8_2_WR8, 48);
		tempWC8_2_WR8[50] = tempWC8[578];
		int[] array = new int[11]
		{
			0, 300, 328, 356, 384, 412, 0, 440, 468, 496,
			524
		};
		int[] array2 = new int[11]
		{
			0, 74, 102, 130, 158, 186, 0, 214, 242, 270,
			298
		};
		int num = 0;
		for (int i = 0; i < 27; i++)
		{
			tempWC8_2_WR8[i + 72] = tempWC8[i + array[sav.Language]];
			num += tempWC8[i + array[sav.Language]];
		}
		if (num == 0)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(sav.OT);
			int num2 = 0;
			byte[] array3 = bytes;
			for (int j = 0; j < array3.Length; j++)
			{
				byte b = (tempWC8_2_WR8[num2 + 72] = array3[j]);
				num2++;
			}
		}
		if ((num == 0) | (tempWC8[626] == 3))
		{
			tempWC8_2_WR8[52] = (byte)sav.Gender;
		}
		else
		{
			tempWC8_2_WR8[52] = tempWC8[626];
		}
		if (tempWC8[array2[sav.Language]] == 0)
		{
			tempWC8_2_WR8[98] = (byte)sav.Language;
		}
		else
		{
			tempWC8_2_WR8[98] = tempWC8[array2[sav.Language]];
		}
		ushort value3 = BitConverter.ToUInt16(tempWC8, 560);
		BitConverter.GetBytes(value3).CopyTo(tempWC8_2_WR8, 56);
		ushort value4 = BitConverter.ToUInt16(tempWC8, 562);
		BitConverter.GetBytes(value4).CopyTo(tempWC8_2_WR8, 60);
		ushort value5 = BitConverter.ToUInt16(tempWC8, 564);
		BitConverter.GetBytes(value5).CopyTo(tempWC8_2_WR8, 64);
		ushort value6 = BitConverter.ToUInt16(tempWC8, 566);
		BitConverter.GetBytes(value6).CopyTo(tempWC8_2_WR8, 68);
		tempWC8_2_WR8[99] = tempWC8[604];
		tempWC8_2_WR8[100] = tempWC8[579];
		Write_WR8_to_block(tempWC8_2_WR8);
	}

	private void WC8_Items(byte[] tempWC8_2_WR8, byte[] tempWC8)
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			ushort num2 = BitConverter.ToUInt16(tempWC8, 32 + i * 4);
			BitConverter.GetBytes(num2).CopyTo(tempWC8_2_WR8, 48 + i * 4);
			BitConverter.GetBytes(BitConverter.ToUInt16(tempWC8, 34 + i * 4)).CopyTo(tempWC8_2_WR8, 50 + i * 4);
			if (num2 > 0)
			{
				num++;
			}
		}
		tempWC8_2_WR8[13] = (byte)num;
		Write_WR8_to_block(tempWC8_2_WR8);
	}

	private void Write_WR8_to_block(byte[] tempWR8)
	{
		for (int i = 0; i < 320; i++)
		{
			WRBlock[i + (int)SlotIndex.Value * 320] = tempWR8[i];
		}
		UpdateEntriesLGPE(WRBlock, (int)SlotIndex.Value);
	}

	private void Populate_Alt_Dict()
	{
		alt_dict.Add(new KeyValuePair<string, string>("3_1", "Mega Venusaur"));
		alt_dict.Add(new KeyValuePair<string, string>("6_1", "Mega Charizard X"));
		alt_dict.Add(new KeyValuePair<string, string>("6_2", "Mega Charizard Y"));
		alt_dict.Add(new KeyValuePair<string, string>("9_1", "Mega Blastoise"));
		alt_dict.Add(new KeyValuePair<string, string>("15_1", "Mega Beedrill"));
		alt_dict.Add(new KeyValuePair<string, string>("18_1", "Mega Pidgeot"));
		alt_dict.Add(new KeyValuePair<string, string>("19_1", "Alolan Rattata"));
		alt_dict.Add(new KeyValuePair<string, string>("20_1", "Alolan Raticate"));
		alt_dict.Add(new KeyValuePair<string, string>("25_1", "Pikachu (Original Cap)"));
		alt_dict.Add(new KeyValuePair<string, string>("25_2", "Pikachu (Hoenn Cap)"));
		alt_dict.Add(new KeyValuePair<string, string>("25_3", "Pikachu (Sinnoh Cap)"));
		alt_dict.Add(new KeyValuePair<string, string>("25_4", "Pikachu (Unova Cap)"));
		alt_dict.Add(new KeyValuePair<string, string>("25_5", "Pikachu (Kalos Cap)"));
		alt_dict.Add(new KeyValuePair<string, string>("25_6", "Pikachu (Alola Cap)"));
		alt_dict.Add(new KeyValuePair<string, string>("25_7", "Pikachu (Partner Cap)"));
		alt_dict.Add(new KeyValuePair<string, string>("25_9", "Pikachu (World Cap)"));
		alt_dict.Add(new KeyValuePair<string, string>("26_1", "Raichu (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("27_1", "Sandshrew (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("28_1", "Sandslash (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("37_1", "Vulpix (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("38_1", "Ninetales(Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("50_1", "Diglett (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("51_1", "Dugtrio (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("52_1", "Meowth (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("52_2", "Meowth (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("53_1", "Persian (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("65_1", "Mega Alakazam"));
		alt_dict.Add(new KeyValuePair<string, string>("74_1", "Alolan Geodude"));
		alt_dict.Add(new KeyValuePair<string, string>("75_1", "Alolan Graveler"));
		alt_dict.Add(new KeyValuePair<string, string>("76_1", "Alolan Golem"));
		alt_dict.Add(new KeyValuePair<string, string>("77_1", "Ponyta (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("78_1", "Rapidash (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("79_1", "Slowpoke (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("80_2", "Slowbro (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("80_1", "Mega Slowbro"));
		alt_dict.Add(new KeyValuePair<string, string>("83_0", "Farfetch'd"));
		alt_dict.Add(new KeyValuePair<string, string>("83_1", "Farfetch'd (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("88_1", "Alolan Grimer"));
		alt_dict.Add(new KeyValuePair<string, string>("89_1", "Alolan Muk"));
		alt_dict.Add(new KeyValuePair<string, string>("94_1", "Mega Gengar"));
		alt_dict.Add(new KeyValuePair<string, string>("103_1", "Exeggutor (Alola)"));
		alt_dict.Add(new KeyValuePair<string, string>("110_1", "Weezing (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("115_1", "Mega Kangaskhan"));
		alt_dict.Add(new KeyValuePair<string, string>("122_0", "Mr. Mime"));
		alt_dict.Add(new KeyValuePair<string, string>("122_1", "Mr. Mime (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("127_1", "Mega Pinsir"));
		alt_dict.Add(new KeyValuePair<string, string>("130_1", "Mega Gyarados"));
		alt_dict.Add(new KeyValuePair<string, string>("142_1", "Mega Aerodactyl"));
		alt_dict.Add(new KeyValuePair<string, string>("144_1", "Articuno (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("145_1", "Zapdos (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("146_1", "Moltres (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("150_1", "Mega Mewtwo X"));
		alt_dict.Add(new KeyValuePair<string, string>("150_2", "Mega Mewtwo Y"));
		alt_dict.Add(new KeyValuePair<string, string>("181_1", "Mega Ampharos"));
		alt_dict.Add(new KeyValuePair<string, string>("199_1", "Slowking (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_0", "Unown (A)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_1", "Unown (B)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_10", "Unown (K)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_11", "Unown (L)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_12", "Unown (M)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_13", "Unown (N)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_14", "Unown (O)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_15", "Unown (P)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_16", "Unown (Q)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_17", "Unown (R)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_18", "Unown (S)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_19", "Unown (T)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_2", "Unown (C)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_20", "Unown (U)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_21", "Unown (V)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_22", "Unown (W)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_23", "Unown (X)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_24", "Unown (Y)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_25", "Unown (Z)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_26", "Unown (!)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_27", "Unown (?)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_3", "Unown (D)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_4", "Unown (E)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_5", "Unown (F)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_6", "Unown (G)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_7", "Unown (H)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_8", "Unown (I)"));
		alt_dict.Add(new KeyValuePair<string, string>("201_9", "Unown (J)"));
		alt_dict.Add(new KeyValuePair<string, string>("208_1", "Mega Steelix"));
		alt_dict.Add(new KeyValuePair<string, string>("212_1", "Mega Scizor"));
		alt_dict.Add(new KeyValuePair<string, string>("214_1", "Mega Heracross"));
		alt_dict.Add(new KeyValuePair<string, string>("222_1", "Corsola (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("248_1", "Mega Tyranitar"));
		alt_dict.Add(new KeyValuePair<string, string>("254_1", "Mega Sceptile"));
		alt_dict.Add(new KeyValuePair<string, string>("257_1", "Mega Blaziken"));
		alt_dict.Add(new KeyValuePair<string, string>("260_1", "Mega Swampert"));
		alt_dict.Add(new KeyValuePair<string, string>("263_1", "Zigzagoon (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("264_1", "Linoone (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("282_1", "Mega Gardevoir"));
		alt_dict.Add(new KeyValuePair<string, string>("299_1", "Mega Houndoom"));
		alt_dict.Add(new KeyValuePair<string, string>("302_1", "Mega Sableye"));
		alt_dict.Add(new KeyValuePair<string, string>("303_1", "Mega Mawile"));
		alt_dict.Add(new KeyValuePair<string, string>("306_1", "Mega Aggron"));
		alt_dict.Add(new KeyValuePair<string, string>("308_1", "Mega Medicham"));
		alt_dict.Add(new KeyValuePair<string, string>("310_1", "Mega Manectric"));
		alt_dict.Add(new KeyValuePair<string, string>("319_1", "Mega Sharpedo"));
		alt_dict.Add(new KeyValuePair<string, string>("323_1", "Mega Camerupt"));
		alt_dict.Add(new KeyValuePair<string, string>("334_1", "Mega Altaria"));
		alt_dict.Add(new KeyValuePair<string, string>("351_0", "Castform"));
		alt_dict.Add(new KeyValuePair<string, string>("351_1", "Sunny Castform"));
		alt_dict.Add(new KeyValuePair<string, string>("351_2", "Rainy Castform"));
		alt_dict.Add(new KeyValuePair<string, string>("351_3", "Snowy Castform"));
		alt_dict.Add(new KeyValuePair<string, string>("354_1", "Mega Banette"));
		alt_dict.Add(new KeyValuePair<string, string>("359_1", "Mega Absol"));
		alt_dict.Add(new KeyValuePair<string, string>("362_1", "Mega Glalie"));
		alt_dict.Add(new KeyValuePair<string, string>("373_1", "Mega Salamance"));
		alt_dict.Add(new KeyValuePair<string, string>("376_1", "Mega Metagross"));
		alt_dict.Add(new KeyValuePair<string, string>("380_1", "Mega Latias"));
		alt_dict.Add(new KeyValuePair<string, string>("381_1", "Mega Latios"));
		alt_dict.Add(new KeyValuePair<string, string>("382_1", "Primal Kyogre"));
		alt_dict.Add(new KeyValuePair<string, string>("383_1", "Primal Groudon"));
		alt_dict.Add(new KeyValuePair<string, string>("384_1", "Mega Rayquaza"));
		alt_dict.Add(new KeyValuePair<string, string>("386_0", "Normal Deoxys"));
		alt_dict.Add(new KeyValuePair<string, string>("386_1", "Attack Deoxys"));
		alt_dict.Add(new KeyValuePair<string, string>("386_2", "Defense Deoxys"));
		alt_dict.Add(new KeyValuePair<string, string>("386_3", "Speed Deoxys"));
		alt_dict.Add(new KeyValuePair<string, string>("412_0", "Plant Burmy"));
		alt_dict.Add(new KeyValuePair<string, string>("412_1", "Sandy Burmy"));
		alt_dict.Add(new KeyValuePair<string, string>("412_2", "Trash Burmy"));
		alt_dict.Add(new KeyValuePair<string, string>("413_0", "Plant Wormadam"));
		alt_dict.Add(new KeyValuePair<string, string>("413_1", "Sandy Wormadam"));
		alt_dict.Add(new KeyValuePair<string, string>("413_2", "Trash Wormadam"));
		alt_dict.Add(new KeyValuePair<string, string>("421_1", "Cherrim (Sunny)"));
		alt_dict.Add(new KeyValuePair<string, string>("421_0", "Overcast Cherrim"));
		alt_dict.Add(new KeyValuePair<string, string>("422_1", "Shellos (East)"));
		alt_dict.Add(new KeyValuePair<string, string>("422_0", "West Shellos"));
		alt_dict.Add(new KeyValuePair<string, string>("423_1", "Gastrodon (East)"));
		alt_dict.Add(new KeyValuePair<string, string>("423_0", "West Gastrodon"));
		alt_dict.Add(new KeyValuePair<string, string>("427_1", "Mega Lopunny"));
		alt_dict.Add(new KeyValuePair<string, string>("445_1", "Mega Garchomp"));
		alt_dict.Add(new KeyValuePair<string, string>("448_1", "Mega Lucario"));
		alt_dict.Add(new KeyValuePair<string, string>("460_1", "Mega Abomasnow"));
		alt_dict.Add(new KeyValuePair<string, string>("475_1", "Mega Gallade"));
		alt_dict.Add(new KeyValuePair<string, string>("479_1", "Rotom (Heat)"));
		alt_dict.Add(new KeyValuePair<string, string>("479_2", "Rotom (Wash)"));
		alt_dict.Add(new KeyValuePair<string, string>("479_3", "Rotom (Frost)"));
		alt_dict.Add(new KeyValuePair<string, string>("479_4", "Rotom (Fan)"));
		alt_dict.Add(new KeyValuePair<string, string>("479_5", "Rotom (Mow)"));
		alt_dict.Add(new KeyValuePair<string, string>("479_0", "Rotom"));
		alt_dict.Add(new KeyValuePair<string, string>("487_0", "Altered Giratina"));
		alt_dict.Add(new KeyValuePair<string, string>("487_1", "Origin Giratina"));
		alt_dict.Add(new KeyValuePair<string, string>("492_0", "Land Shaymin"));
		alt_dict.Add(new KeyValuePair<string, string>("492_1", "Sky Shaymin"));
		alt_dict.Add(new KeyValuePair<string, string>("531_1", "Mega Audino"));
		alt_dict.Add(new KeyValuePair<string, string>("550_1", "Basculin (Blue)"));
		alt_dict.Add(new KeyValuePair<string, string>("550_0", "Red Basculin"));
		alt_dict.Add(new KeyValuePair<string, string>("554_1", "Darumaka (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("555_1", "Darmanitan (Zen)"));
		alt_dict.Add(new KeyValuePair<string, string>("555_3", "Darmanitan (Zen Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("555_2", "Darmanitan (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("562_1", "Yamask (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("618_1", "Stunfisk (Galar)"));
		alt_dict.Add(new KeyValuePair<string, string>("641_0", "Incarnate Tornadus"));
		alt_dict.Add(new KeyValuePair<string, string>("641_1", "Therian Tornadus"));
		alt_dict.Add(new KeyValuePair<string, string>("642_0", "Incarnate Thundurus"));
		alt_dict.Add(new KeyValuePair<string, string>("642_1", "Therian Thundurus"));
		alt_dict.Add(new KeyValuePair<string, string>("645_0", "Incarnate Landorus"));
		alt_dict.Add(new KeyValuePair<string, string>("645_1", "Therian Landorus"));
		alt_dict.Add(new KeyValuePair<string, string>("646_1", "Kyurem (White)"));
		alt_dict.Add(new KeyValuePair<string, string>("646_2", "Kyurem (Black)"));
		alt_dict.Add(new KeyValuePair<string, string>("646_0", "Kyurem"));
		alt_dict.Add(new KeyValuePair<string, string>("647_1", "Keldeo (Resolute)"));
		alt_dict.Add(new KeyValuePair<string, string>("647_0", "Keldeo"));
		alt_dict.Add(new KeyValuePair<string, string>("648_0", "Aria Meloetta"));
		alt_dict.Add(new KeyValuePair<string, string>("648_1", "Pirouette Meloetta"));
		alt_dict.Add(new KeyValuePair<string, string>("658_1", "Ash-Greninja"));
		alt_dict.Add(new KeyValuePair<string, string>("658_2", "Ash-Greninja"));
		alt_dict.Add(new KeyValuePair<string, string>("666_0", "Icy Snow Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_1", "Polar Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_10", "High Plains Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_11", "Sandstorm Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_12", "River Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_13", "Monsoon Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_14", "Savanna Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_15", "Sun Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_16", "Ocean Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_17", "Jungle Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_18", "Fancy Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_19", "Poke Ball Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_2", "Tundra Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_3", "Continental Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_4", "Garden Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_5", "Elegant Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_6", "Meadow Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_7", "Modern Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_8", "Marine Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("666_9", "Archipelago Vivillon"));
		alt_dict.Add(new KeyValuePair<string, string>("669_0", "Red Flabébé"));
		alt_dict.Add(new KeyValuePair<string, string>("669_1", "Yellow Flabébé"));
		alt_dict.Add(new KeyValuePair<string, string>("669_2", "Orange Flabébé"));
		alt_dict.Add(new KeyValuePair<string, string>("669_3", "Blue Flabébé"));
		alt_dict.Add(new KeyValuePair<string, string>("669_4", "White Flabébé"));
		alt_dict.Add(new KeyValuePair<string, string>("670_1", "Yellow Floette"));
		alt_dict.Add(new KeyValuePair<string, string>("670_2", "Orange Floette"));
		alt_dict.Add(new KeyValuePair<string, string>("670_3", "Blue Floette"));
		alt_dict.Add(new KeyValuePair<string, string>("670_4", "White Floette"));
		alt_dict.Add(new KeyValuePair<string, string>("670_5", "Eternal Floette"));
		alt_dict.Add(new KeyValuePair<string, string>("671_0", "Red Florges"));
		alt_dict.Add(new KeyValuePair<string, string>("671_1", "Yellow Florges"));
		alt_dict.Add(new KeyValuePair<string, string>("671_2", "Orange Florges"));
		alt_dict.Add(new KeyValuePair<string, string>("671_3", "Blue Florges"));
		alt_dict.Add(new KeyValuePair<string, string>("671_4", "White Florges"));
		alt_dict.Add(new KeyValuePair<string, string>("676_1", "Heart Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("676_2", "Star Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("676_3", "Diamond Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("676_4", "Debutante Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("676_5", "Matron Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("676_6", "Dandy Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("676_7", "La Reine Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("676_8", "Kabuki Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("676_9", "Pharoah Furfrou"));
		alt_dict.Add(new KeyValuePair<string, string>("678_0", "Meowstic (M)"));
		alt_dict.Add(new KeyValuePair<string, string>("678_1", "Meowstic (F)"));
		alt_dict.Add(new KeyValuePair<string, string>("681_0", "Aegislash (Shield)"));
		alt_dict.Add(new KeyValuePair<string, string>("681_1", "Aegislash (Blade)"));
		alt_dict.Add(new KeyValuePair<string, string>("710_0", "Average Pumpkaboo"));
		alt_dict.Add(new KeyValuePair<string, string>("710_1", "Pumpkaboo (Small)"));
		alt_dict.Add(new KeyValuePair<string, string>("710_2", "Pumpkaboo (Large)"));
		alt_dict.Add(new KeyValuePair<string, string>("710_3", "Pumpkaboo (Super)"));
		alt_dict.Add(new KeyValuePair<string, string>("711_0", "Average Gourgeist"));
		alt_dict.Add(new KeyValuePair<string, string>("711_1", "Gourgeist (Small)"));
		alt_dict.Add(new KeyValuePair<string, string>("711_2", "Gourgeist (Large)"));
		alt_dict.Add(new KeyValuePair<string, string>("711_3", "Gourgeist (Super)"));
		alt_dict.Add(new KeyValuePair<string, string>("718_1", "10% Zygarde"));
		alt_dict.Add(new KeyValuePair<string, string>("718_2", "10% Zygarde"));
		alt_dict.Add(new KeyValuePair<string, string>("718_3", "50% Zygarde"));
		alt_dict.Add(new KeyValuePair<string, string>("718_4", "Perfect Zygarde"));
		alt_dict.Add(new KeyValuePair<string, string>("719_1", "Mega Diancie"));
		alt_dict.Add(new KeyValuePair<string, string>("720_1", "Unbound Hoopa"));
		alt_dict.Add(new KeyValuePair<string, string>("741_0", "Baile Oricorio"));
		alt_dict.Add(new KeyValuePair<string, string>("741_1", "Pom-pom Oricorio"));
		alt_dict.Add(new KeyValuePair<string, string>("741_2", "Pa'u Oricorio"));
		alt_dict.Add(new KeyValuePair<string, string>("741_3", "Sensu Oricorio"));
		alt_dict.Add(new KeyValuePair<string, string>("744_1", "Rockruff (Own Tempo)"));
		alt_dict.Add(new KeyValuePair<string, string>("745_0", "Midday Lycanroc"));
		alt_dict.Add(new KeyValuePair<string, string>("745_1", "Lycanroc (Midnight)"));
		alt_dict.Add(new KeyValuePair<string, string>("745_2", "Lycanroc (Dusk)"));
		alt_dict.Add(new KeyValuePair<string, string>("746_1", "Wishiwashi (School)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_0", "Silvally (Normal)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_1", "Silvally (Fighting)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_10", "Silvally (Water)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_11", "Silvally (Grass)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_12", "Silvally (Electric)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_13", "Silvally (Psychic)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_14", "Silvally (Ice)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_15", "Silvally (Dragon)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_16", "Silvally (Dark)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_17", "Silvally (Fairy)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_2", "Silvally (Flying)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_3", "Silvally (Poison)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_4", "Silvally (Ground)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_5", "Silvally (Rock)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_6", "Silvally (Bug)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_7", "Silvally (Ghost)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_8", "Silvally (Steel)"));
		alt_dict.Add(new KeyValuePair<string, string>("773_9", "Silvally (Fire)"));
		alt_dict.Add(new KeyValuePair<string, string>("774_1", "Orange Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_10", "Green Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_11", "Blue Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_12", "Indigo Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_13", "Violet Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_2", "Yellow Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_3", "Green Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_4", "Blue Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_5", "Indigo Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_6", "Violet Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_7", "Red Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_8", "Orange Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("774_9", "Yellow Minior"));
		alt_dict.Add(new KeyValuePair<string, string>("778_0", "Mimikyu (Disguised)"));
		alt_dict.Add(new KeyValuePair<string, string>("778_1", "Mimikyu (Busted)"));
		alt_dict.Add(new KeyValuePair<string, string>("800_1", "Necrozma (Dusk Mane)"));
		alt_dict.Add(new KeyValuePair<string, string>("800_2", "Necrozma (Dawn Wings)"));
		alt_dict.Add(new KeyValuePair<string, string>("800_3", "Ultra Necrozma"));
		alt_dict.Add(new KeyValuePair<string, string>("801_1", "Magearna (Poke Ball Colors)"));
		alt_dict.Add(new KeyValuePair<string, string>("845_1", "Cramorant (Gulping)"));
		alt_dict.Add(new KeyValuePair<string, string>("845_2", "Cramorant (Gorging)"));
		alt_dict.Add(new KeyValuePair<string, string>("849_0", "Toxtricity (Amped)"));
		alt_dict.Add(new KeyValuePair<string, string>("849_1", "Toxtricity (Low Key)"));
		alt_dict.Add(new KeyValuePair<string, string>("854_0", "Sinistea (Phony)"));
		alt_dict.Add(new KeyValuePair<string, string>("854_1", "Sinistea (Antique)"));
		alt_dict.Add(new KeyValuePair<string, string>("855_0", "Polteageist (Phony)"));
		alt_dict.Add(new KeyValuePair<string, string>("855_1", "Polteageist (Antique)"));
		alt_dict.Add(new KeyValuePair<string, string>("862_0", "Obstagoon"));
		alt_dict.Add(new KeyValuePair<string, string>("863_0", "Perrserker"));
		alt_dict.Add(new KeyValuePair<string, string>("865_0", "Sirfetch'd"));
		alt_dict.Add(new KeyValuePair<string, string>("866_0", "Mr. Rime"));
		alt_dict.Add(new KeyValuePair<string, string>("867_0", "Runerigus"));
		alt_dict.Add(new KeyValuePair<string, string>("869_0", "Alcremie (Vanilla Cream)"));
		alt_dict.Add(new KeyValuePair<string, string>("869_1", "Alcremie (Ruby Cream)"));
		alt_dict.Add(new KeyValuePair<string, string>("869_2", "Alcremie (Matcha Cream)"));
		alt_dict.Add(new KeyValuePair<string, string>("869_3", "Alcremie (Mint Cream)"));
		alt_dict.Add(new KeyValuePair<string, string>("869_4", "Alcremie (Lemon Cream)"));
		alt_dict.Add(new KeyValuePair<string, string>("869_5", "Alcremie (Salted Cream)"));
		alt_dict.Add(new KeyValuePair<string, string>("869_6", "Alcremie (Ruby Swirl)"));
		alt_dict.Add(new KeyValuePair<string, string>("869_7", "Alcremie (Caramel Swirl)"));
		alt_dict.Add(new KeyValuePair<string, string>("869_8", "Alcremie (Rainbow Swirl)"));
		alt_dict.Add(new KeyValuePair<string, string>("875_0", "Eiscue (Ice Face)"));
		alt_dict.Add(new KeyValuePair<string, string>("875_1", "Eiscue (Noice Face)"));
		alt_dict.Add(new KeyValuePair<string, string>("876_0", "Indeedee (M)"));
		alt_dict.Add(new KeyValuePair<string, string>("876_1", "Indeedee (F)"));
		alt_dict.Add(new KeyValuePair<string, string>("877_0", "Morpeko (Full Belly)"));
		alt_dict.Add(new KeyValuePair<string, string>("877_1", "Morpeko (Hangry)"));
		alt_dict.Add(new KeyValuePair<string, string>("888_0", "Zacian (Hero of Many Battles)"));
		alt_dict.Add(new KeyValuePair<string, string>("888_1", "Zacian (Crowned Sword)"));
		alt_dict.Add(new KeyValuePair<string, string>("889_0", "Zamazenta (Hero of Many Battles)"));
		alt_dict.Add(new KeyValuePair<string, string>("889_1", "Zamazenta (Crowned Shield)"));
		alt_dict.Add(new KeyValuePair<string, string>("890_1", "Eternatus (Eternamax)"));
		alt_dict.Add(new KeyValuePair<string, string>("892_0", "Urshifu (Single Strike)"));
		alt_dict.Add(new KeyValuePair<string, string>("892_1", "Urshifu (Rapid Strike)"));
		alt_dict.Add(new KeyValuePair<string, string>("893_1", "Zarude (Dada)"));
		alt_dict.Add(new KeyValuePair<string, string>("898_1", "Calyrex (Ice Rider)"));
		alt_dict.Add(new KeyValuePair<string, string>("898_2", "Calyrex (Shadow Rider)"));
		alt_dex.Add(new KeyValuePair<string, string>("77_1", "Unique Horn Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("78_1", "Unique Horn Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("122_1", "Dancing Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("144_1", "Cruel Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("145_1", "Strong Legs Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("146_1", "Malevolent Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("199_1", "Hexpert Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("555_2", "Zen Charm Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("720_1", "Djinn Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("898_1", "High King Pokémon"));
		alt_dex.Add(new KeyValuePair<string, string>("898_2", "High King Pokémon"));
	}

	private string NameFinder()
	{
		string text = "";
		if (alt_dict.ContainsKey(SpeciesBox.SelectedIndex + "_" + FormBox.Value))
		{
			return alt_dict[SpeciesBox.SelectedIndex + "_" + FormBox.Value];
		}
		return SpeciesBox.Text;
	}

	private string AltDexFinder()
	{
		string text = "";
		if (alt_dex.ContainsKey(SpeciesBox.SelectedIndex + "_" + FormBox.Value))
		{
			return alt_dex[SpeciesBox.SelectedIndex + "_" + FormBox.Value];
		}
		pokedextype.SelectedIndex = SpeciesBox.SelectedIndex;
		return pokedextype.Text;
	}

	private void TimestampBox_TextChanged(object sender, EventArgs e)
	{
		if (!(InitialLoad | EditingTimeStamp))
		{
		}
	}

	private void TimeStampReader()
	{
		if (string.IsNullOrEmpty(TimestampBox.Text) | (TimestampBox.Text == "0") | !ulong.TryParse(TimestampBox.Text, out var _))
		{
			LoadingTimeStamp = true;
			NumericUpDown numericUpDown = YearBox;
			for (int i = 0; i < 6; i++)
			{
				switch (i)
				{
				case 0:
					numericUpDown = YearBox;
					break;
				case 1:
					numericUpDown = MonthBox;
					break;
				case 2:
					numericUpDown = DayBox;
					break;
				case 3:
					numericUpDown = HourBox;
					break;
				case 4:
					numericUpDown = MinBox;
					break;
				case 5:
					numericUpDown = SecBox;
					break;
				}
				numericUpDown.Value = numericUpDown.Minimum;
			}
			LoadingTimeStamp = false;
		}
		else
		{
			LoadingTimeStamp = true;
			YearBox.Value = (ulong.Parse(TimestampBox.Text) >> 26) & 0xFFF;
			MonthBox.Value = (ulong.Parse(TimestampBox.Text) >> 22) & 0xF;
			DayBox.Value = (ulong.Parse(TimestampBox.Text) >> 17) & 0x1F;
			HourBox.Value = (ulong.Parse(TimestampBox.Text) >> 12) & 0x1F;
			MinBox.Value = (ulong.Parse(TimestampBox.Text) >> 6) & 0x3F;
			SecBox.Value = ulong.Parse(TimestampBox.Text) & 0x3F;
			LoadingTimeStamp = false;
		}
	}

	private void TimeStampWriter()
	{
		if (LoadingTimeStamp)
		{
			return;
		}
		ulong num = 0uL;
		EditingTimeStamp = true;
		NumericUpDown numericUpDown = YearBox;
		int[] array = new int[6] { 26, 22, 17, 12, 6, 0 };
		for (int i = 0; i < 6; i++)
		{
			switch (i)
			{
			case 0:
				numericUpDown = YearBox;
				break;
			case 1:
				numericUpDown = MonthBox;
				break;
			case 2:
				numericUpDown = DayBox;
				break;
			case 3:
				numericUpDown = HourBox;
				break;
			case 4:
				numericUpDown = MinBox;
				break;
			case 5:
				numericUpDown = SecBox;
				break;
			}
			if (string.IsNullOrEmpty(numericUpDown.Text))
			{
				numericUpDown.Value = numericUpDown.Minimum;
			}
			else
			{
				num += (ulong)numericUpDown.Value << array[i];
			}
		}
		TimestampBox.Text = num.ToString();
		WriteTimestamp(temparray);
		Write_WR8_to_block(temparray);
		EditingTimeStamp = false;
	}

	private void YearBox_ValueChanged(object sender, EventArgs e)
	{
		TimeStampWriter();
	}

	private void MonthBox_ValueChanged(object sender, EventArgs e)
	{
		TimeStampWriter();
	}

	private void DayBox_ValueChanged(object sender, EventArgs e)
	{
		TimeStampWriter();
	}

	private void HourBox_ValueChanged(object sender, EventArgs e)
	{
		TimeStampWriter();
	}

	private void MinBox_ValueChanged(object sender, EventArgs e)
	{
		TimeStampWriter();
	}

	private void SecBox_ValueChanged(object sender, EventArgs e)
	{
		TimeStampWriter();
	}

	private void DateNul_Click(object sender, EventArgs e)
	{
		EditingTimeStamp = true;
		LoadingTimeStamp = true;
		TimestampBox.Text = "0";
		NumericUpDown numericUpDown = YearBox;
		for (int i = 0; i < 6; i++)
		{
			switch (i)
			{
			case 0:
				numericUpDown = YearBox;
				break;
			case 1:
				numericUpDown = MonthBox;
				break;
			case 2:
				numericUpDown = DayBox;
				break;
			case 3:
				numericUpDown = HourBox;
				break;
			case 4:
				numericUpDown = MinBox;
				break;
			case 5:
				numericUpDown = SecBox;
				break;
			}
			numericUpDown.Value = numericUpDown.Minimum;
		}
		WriteTimestamp(temparray);
		Write_WR8_to_block(temparray);
		EditingTimeStamp = false;
		LoadingTimeStamp = false;
	}

	private void InsertWR8Button_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog
		{
			Filter = "WR8 files (*.wr8)|*.wr8",
			FilterIndex = 0,
			RestoreDirectory = true
		};
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			Insert_WR8(openFileDialog.FileNames[0]);
		}
	}

	private void Insert_WR8(string path)
	{
	}

	private void WriteTimestamp(byte[] tempWC8_2_WR8)
	{
		ulong result;
		if (string.IsNullOrEmpty(TimestampBox.Text))
		{
			BitConverter.GetBytes(Convert.ToUInt32(0)).CopyTo(tempWC8_2_WR8, 0);
		}
		else if (!ulong.TryParse(TimestampBox.Text, out result))
		{
			BitConverter.GetBytes(Convert.ToUInt64(0)).CopyTo(tempWC8_2_WR8, 0);
		}
		else if (Convert.ToUInt64(TimestampBox.Text) > ulong.MaxValue)
		{
			BitConverter.GetBytes(Convert.ToUInt64(0)).CopyTo(tempWC8_2_WR8, 0);
		}
		else
		{
			BitConverter.GetBytes(Convert.ToUInt64(TimestampBox.Text)).CopyTo(tempWC8_2_WR8, 0);
		}
	}

	private void ExtractWR8Button_Click(object sender, EventArgs e)
	{
	}

	private void DeleteWR8Button_Click(object sender, EventArgs e)
	{
		Array.Clear(temparray, 0, temparray.Length);
		Write_WR8_to_block(temparray);
	}

	private void WonderRecordForm_DragEnter(object sender, DragEventArgs e)
	{
	}

	private void WonderRecordForm_DragDrop(object sender, DragEventArgs e)
	{
	}

	private void WC8orWR8(string path)
	{
		byte[] array = File.ReadAllBytes(path);
		switch (array.Length)
		{
		case 720:
			WC8_Load(path);
			break;
		case 104:
			Insert_WR8(path);
			break;
		default:
			SystemSounds.Asterisk.Play();
			break;
		}
	}

	private void Border_Change(object sender, EventArgs e)
	{
		string s = ((PictureBox)sender).Name.Replace("pictureBox", "");
		ClickedSlot = ushort.Parse(s) - 1;
		SlotIndex.Value = ClickedSlot;
		int num = 0;
		for (num = 0; num < 10; num++)
		{
			((PictureBox)base.Controls["pictureBox" + (num + 1)]).BorderStyle = BorderStyle.FixedSingle;
		}
		((PictureBox)base.Controls["pictureBox" + (ClickedSlot + 1)]).BorderStyle = BorderStyle.Fixed3D;
	}

	private void loadslots()
	{
		int num = 0;
		for (num = 0; num < 10; num++)
		{
			byte[] array = new byte[320];
			for (int i = 0; i < 320; i++)
			{
				array[i] = WRBlock[i + 320 * num];
			}
			((PictureBox)base.Controls["pictureBox" + (num + 1)]).Image = ImageSelector(array);
		}
	}

	public string GetItemResourceName(int item)
	{
		DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 1);
		defaultInterpolatedStringHandler.AppendLiteral("item_");
		defaultInterpolatedStringHandler.AppendFormatted(item);
		return "b" + defaultInterpolatedStringHandler.ToStringAndClear();
	}

	public Image GetItemImage(int tempitem)
	{
		Image image = (Image)PKHeX.Drawing.PokeSprite.Properties.Resources.ResourceManager.GetObject(GetItemResourceName(tempitem));
		if (tempitem >= 1130 && tempitem <= 1229)
		{
			image = PKHeX.Drawing.PokeSprite.Properties.Resources.bitem_tr;
		}
		else if (tempitem >= 328 && tempitem <= 419)
		{
			image = PKHeX.Drawing.PokeSprite.Properties.Resources.bitem_tm;
		}
		else if (tempitem >= 1279 && tempitem <= 1578)
		{
			image = Resources.bitem_715;
		}
		if (image == null)
		{
			image = PKHeX.Drawing.PokeSprite.Properties.Resources.bitem_unk;
		}
		return RedrawImage(image);
	}

	public Image RedrawImage(Image itemimg)
	{
		int num = 2;
		int num2 = 2;
		int x = 34 - itemimg.Width / 2;
		int y = 56 - itemimg.Height - num2;
		object obj = itemimg.Clone();
		Bitmap bitmap = new Bitmap(68, 56);
		Graphics graphics = Graphics.FromImage(bitmap);
		graphics.Clear(Color.Transparent);
		graphics.DrawImageUnscaled((Image)obj, x, y, 68, 56);
		return bitmap;
	}

	private void SpeciesImageSelector()
	{
		Image image = ImageSelector(temparray);
		if (image == null)
		{
			SpeciesImageBox.Image = null;
		}
		else
		{
			SpeciesImageBox.Image = ResizeBitmap(new Bitmap(image), 136, 112);
		}
	}

	private Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
	{
		Bitmap bitmap = new Bitmap(width, height);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
			graphics.DrawImage(sourceBMP, 0, 0, width, height);
		}
		return bitmap;
	}

	private Image ImageSelector(byte[] temp_wr_array)
	{
		Image image = null;
		int num = temp_wr_array[12];
		ushort num2 = BitConverter.ToUInt16(temp_wr_array, 268);
		int num3 = temp_wr_array[10];
		int tempitem = BitConverter.ToUInt16(temp_wr_array, 268);
		Shiny shiny = Shiny.Never;
		if (num3 == 6)
		{
			shiny = Shiny.Always;
		}
		switch (num)
		{
		case 0:
			return null;
		case 1:
			image = SpriteUtil.GetSprite(num2, (byte)0, 0, 0u, 0, false, shiny, EntityContext.None);
			break;
		case 2:
			image = GetItemImage(tempitem);
			break;
		default:
			image = Resources.b_unknown;
			break;
		}
		return image;
	}

	private void SpeciesBox_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void otnamebox_TextChanged(object sender, EventArgs e)
	{
	}

	private void CardTitleRefinedBox_TextChanged(object sender, EventArgs e)
	{
	}

	private void CardTitleRawBox_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.LocationLabel = new System.Windows.Forms.Label();
		this.SlotIndex = new System.Windows.Forms.NumericUpDown();
		this.SpeciesBox = new System.Windows.Forms.ComboBox();
		this.AreaSlotLabel = new System.Windows.Forms.Label();
		this.SpeciesLabel = new System.Windows.Forms.Label();
		this.FormBox = new System.Windows.Forms.NumericUpDown();
		this.SpeciesImageBox = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.EntryTypeBox = new System.Windows.Forms.ComboBox();
		this.WCIDBox = new System.Windows.Forms.NumericUpDown();
		this.label2 = new System.Windows.Forms.Label();
		this.CardTitleRawBox = new System.Windows.Forms.ComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.WRTabs = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.RefinedSpeciesBox = new System.Windows.Forms.TextBox();
		this.move1list = new System.Windows.Forms.ComboBox();
		this.Label28 = new System.Windows.Forms.Label();
		this.move2list = new System.Windows.Forms.ComboBox();
		this.move3list = new System.Windows.Forms.ComboBox();
		this.move4list = new System.Windows.Forms.ComboBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.Label4 = new System.Windows.Forms.Label();
		this.pokedextype = new System.Windows.Forms.ComboBox();
		this.Label5 = new System.Windows.Forms.Label();
		this.Label21 = new System.Windows.Forms.Label();
		this.Label6 = new System.Windows.Forms.Label();
		this.pokemonlc = new System.Windows.Forms.TextBox();
		this.otnamebox = new System.Windows.Forms.TextBox();
		this.OTG = new System.Windows.Forms.ComboBox();
		this.languagebox = new System.Windows.Forms.ComboBox();
		this.Label7 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.itemnameplural = new System.Windows.Forms.ComboBox();
		this.nitem6 = new System.Windows.Forms.TextBox();
		this.nitem5 = new System.Windows.Forms.TextBox();
		this.nitem4 = new System.Windows.Forms.TextBox();
		this.nitem3 = new System.Windows.Forms.TextBox();
		this.nitem2 = new System.Windows.Forms.TextBox();
		this.nitem1 = new System.Windows.Forms.TextBox();
		this.itemslc = new System.Windows.Forms.TextBox();
		this.Label20 = new System.Windows.Forms.Label();
		this.Label18 = new System.Windows.Forms.Label();
		this.Label17 = new System.Windows.Forms.Label();
		this.Label16 = new System.Windows.Forms.Label();
		this.Label15 = new System.Windows.Forms.Label();
		this.Label14 = new System.Windows.Forms.Label();
		this.itembox6 = new System.Windows.Forms.ComboBox();
		this.itembox5 = new System.Windows.Forms.ComboBox();
		this.itembox4 = new System.Windows.Forms.ComboBox();
		this.itembox3 = new System.Windows.Forms.ComboBox();
		this.itembox2 = new System.Windows.Forms.ComboBox();
		this.itembox1 = new System.Windows.Forms.ComboBox();
		this.Label13 = new System.Windows.Forms.Label();
		this.CardTitleRefinedBox = new System.Windows.Forms.TextBox();
		this.WC8_2_WR8_Button = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.TimestampBox = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.DateNul = new System.Windows.Forms.Button();
		this.SecBox = new System.Windows.Forms.NumericUpDown();
		this.label40 = new System.Windows.Forms.Label();
		this.DayBox = new System.Windows.Forms.NumericUpDown();
		this.MinBox = new System.Windows.Forms.NumericUpDown();
		this.MonthBox = new System.Windows.Forms.NumericUpDown();
		this.HourBox = new System.Windows.Forms.NumericUpDown();
		this.YearBox = new System.Windows.Forms.NumericUpDown();
		this.label39 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.InsertWR8Button = new System.Windows.Forms.Button();
		this.ExtractWR8Button = new System.Windows.Forms.Button();
		this.DeleteWR8Button = new System.Windows.Forms.Button();
		this.pictureBox10 = new System.Windows.Forms.PictureBox();
		this.pictureBox9 = new System.Windows.Forms.PictureBox();
		this.pictureBox8 = new System.Windows.Forms.PictureBox();
		this.pictureBox7 = new System.Windows.Forms.PictureBox();
		this.pictureBox6 = new System.Windows.Forms.PictureBox();
		this.pictureBox5 = new System.Windows.Forms.PictureBox();
		this.pictureBox4 = new System.Windows.Forms.PictureBox();
		this.pictureBox3 = new System.Windows.Forms.PictureBox();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.pictureBox0 = new System.Windows.Forms.PictureBox();
		((System.ComponentModel.ISupportInitialize)this.SlotIndex).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.FormBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.SpeciesImageBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.WCIDBox).BeginInit();
		this.WRTabs.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.SecBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.DayBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MinBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.MonthBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.HourBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.YearBox).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox10).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox9).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox8).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox7).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox6).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox4).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox0).BeginInit();
		base.SuspendLayout();
		this.LocationLabel.AutoSize = true;
		this.LocationLabel.Location = new System.Drawing.Point(9, 9);
		this.LocationLabel.Name = "LocationLabel";
		this.LocationLabel.Size = new System.Drawing.Size(0, 13);
		this.LocationLabel.TabIndex = 3;
		this.SlotIndex.Enabled = false;
		this.SlotIndex.Location = new System.Drawing.Point(90, 128);
		this.SlotIndex.Maximum = new decimal(new int[4] { 9, 0, 0, 0 });
		this.SlotIndex.Name = "SlotIndex";
		this.SlotIndex.Size = new System.Drawing.Size(87, 20);
		this.SlotIndex.TabIndex = 1;
		this.SlotIndex.ValueChanged += new System.EventHandler(SlotIndex_ValueChanged);
		this.SpeciesBox.AllowDrop = true;
		this.SpeciesBox.FormattingEnabled = true;
		this.SpeciesBox.Items.AddRange(new object[1] { "1" });
		this.SpeciesBox.Location = new System.Drawing.Point(78, 21);
		this.SpeciesBox.Name = "SpeciesBox";
		this.SpeciesBox.Size = new System.Drawing.Size(154, 21);
		this.SpeciesBox.TabIndex = 1;
		this.SpeciesBox.SelectedIndexChanged += new System.EventHandler(SpeciesBox_SelectedIndexChanged);
		this.AreaSlotLabel.AutoSize = true;
		this.AreaSlotLabel.Location = new System.Drawing.Point(16, 130);
		this.AreaSlotLabel.Name = "AreaSlotLabel";
		this.AreaSlotLabel.Size = new System.Drawing.Size(52, 13);
		this.AreaSlotLabel.TabIndex = 29;
		this.AreaSlotLabel.Text = "Entry Slot";
		this.SpeciesLabel.AutoSize = true;
		this.SpeciesLabel.Location = new System.Drawing.Point(12, 24);
		this.SpeciesLabel.Name = "SpeciesLabel";
		this.SpeciesLabel.Size = new System.Drawing.Size(45, 13);
		this.SpeciesLabel.TabIndex = 32;
		this.SpeciesLabel.Text = "Species";
		this.FormBox.Location = new System.Drawing.Point(452, 24);
		this.FormBox.Maximum = new decimal(new int[4] { 8, 0, 0, 0 });
		this.FormBox.Name = "FormBox";
		this.FormBox.Size = new System.Drawing.Size(87, 20);
		this.FormBox.TabIndex = 13;
		this.FormBox.Visible = false;
		this.SpeciesImageBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.SpeciesImageBox.Location = new System.Drawing.Point(3, 2);
		this.SpeciesImageBox.Name = "SpeciesImageBox";
		this.SpeciesImageBox.Size = new System.Drawing.Size(136, 112);
		this.SpeciesImageBox.TabIndex = 28;
		this.SpeciesImageBox.TabStop = false;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(308, 182);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 13);
		this.label1.TabIndex = 110;
		this.label1.Text = "Entry Type";
		this.EntryTypeBox.AllowDrop = true;
		this.EntryTypeBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.EntryTypeBox.Enabled = false;
		this.EntryTypeBox.FormattingEnabled = true;
		this.EntryTypeBox.Items.AddRange(new object[4] { "None", "Pokémon", "Items", "Unknown" });
		this.EntryTypeBox.Location = new System.Drawing.Point(381, 179);
		this.EntryTypeBox.Name = "EntryTypeBox";
		this.EntryTypeBox.Size = new System.Drawing.Size(87, 21);
		this.EntryTypeBox.TabIndex = 4;
		this.WCIDBox.Enabled = false;
		this.WCIDBox.Location = new System.Drawing.Point(90, 180);
		this.WCIDBox.Maximum = new decimal(new int[4] { 9999, 0, 0, 0 });
		this.WCIDBox.Name = "WCIDBox";
		this.WCIDBox.Size = new System.Drawing.Size(87, 20);
		this.WCIDBox.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(16, 182);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(39, 13);
		this.label2.TabIndex = 413;
		this.label2.Text = "WC ID";
		this.CardTitleRawBox.AllowDrop = true;
		this.CardTitleRawBox.Enabled = false;
		this.CardTitleRawBox.FormattingEnabled = true;
		this.CardTitleRawBox.Items.AddRange(new object[26]
		{
			"[VAR PKNAME(0000)] Gift", "[VAR ITEM2(0008)] Gift", "Item Set Gift", "[VAR 0104(0001)] [VAR PKNAME(0000)] Gift", "Mythical Pokémon [VAR PKNAME(0000)] Gift", "[VAR TRNAME(0003)]’s [VAR PKNAME(0000)] Gift", "Shiny [VAR PKNAME(0000)] Gift", "[VAR PKNAME(0000)] ([VAR 01CA(0002)]) Gift", "[VAR 01CA(0002)] Gift", "Hidden Ability [VAR PKNAME(0000)] Gift",
			"[VAR MOVE(0004)] [VAR PKNAME(0000)] Gift", "[VAR PKNAME(0000)] with [VAR MOVE(0005)] Gift", "[VAR PKNAME(0000)] with [VAR MOVE(0006)] Gift", "[VAR PKNAME(0000)] with [VAR MOVE(0007)] Gift", "[VAR PKNAME(0000)] & [VAR ITEM2(0009)] Gift", "Downloadable Version Bonus", "Special Pack Purchase Bonus", "Store Purchase Bonus", "Strategy Guide Purchase Bonus", "Purchase Bonus",
			"Happy Birthday!", "Virtual Console Bonus", "Pokémon Trainer Club Gift", "Pokémon Global Link Gift", "Pokémon Bank Gift", "Unknown"
		});
		this.CardTitleRawBox.Location = new System.Drawing.Point(90, 159);
		this.CardTitleRawBox.Name = "CardTitleRawBox";
		this.CardTitleRawBox.Size = new System.Drawing.Size(215, 21);
		this.CardTitleRawBox.TabIndex = 3;
		this.CardTitleRawBox.Visible = false;
		this.CardTitleRawBox.SelectedIndexChanged += new System.EventHandler(CardTitleRawBox_SelectedIndexChanged);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(16, 157);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(52, 13);
		this.label3.TabIndex = 415;
		this.label3.Text = "Card Title";
		this.WRTabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
		this.WRTabs.Controls.Add(this.tabPage1);
		this.WRTabs.Controls.Add(this.tabPage2);
		this.WRTabs.Controls.Add(this.tabPage3);
		this.WRTabs.Enabled = false;
		this.WRTabs.Location = new System.Drawing.Point(0, 227);
		this.WRTabs.Name = "WRTabs";
		this.WRTabs.SelectedIndex = 0;
		this.WRTabs.Size = new System.Drawing.Size(521, 266);
		this.WRTabs.TabIndex = 418;
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(513, 237);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "None";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add(this.RefinedSpeciesBox);
		this.tabPage2.Controls.Add(this.move1list);
		this.tabPage2.Controls.Add(this.Label28);
		this.tabPage2.Controls.Add(this.move2list);
		this.tabPage2.Controls.Add(this.move3list);
		this.tabPage2.Controls.Add(this.move4list);
		this.tabPage2.Controls.Add(this.label11);
		this.tabPage2.Controls.Add(this.label10);
		this.tabPage2.Controls.Add(this.Label4);
		this.tabPage2.Controls.Add(this.pokedextype);
		this.tabPage2.Controls.Add(this.Label5);
		this.tabPage2.Controls.Add(this.Label21);
		this.tabPage2.Controls.Add(this.FormBox);
		this.tabPage2.Controls.Add(this.Label6);
		this.tabPage2.Controls.Add(this.pokemonlc);
		this.tabPage2.Controls.Add(this.otnamebox);
		this.tabPage2.Controls.Add(this.OTG);
		this.tabPage2.Controls.Add(this.languagebox);
		this.tabPage2.Controls.Add(this.Label7);
		this.tabPage2.Controls.Add(this.SpeciesBox);
		this.tabPage2.Controls.Add(this.SpeciesLabel);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(513, 237);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Pokémon";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.RefinedSpeciesBox.Enabled = false;
		this.RefinedSpeciesBox.Location = new System.Drawing.Point(403, 107);
		this.RefinedSpeciesBox.MaxLength = 1;
		this.RefinedSpeciesBox.Name = "RefinedSpeciesBox";
		this.RefinedSpeciesBox.Size = new System.Drawing.Size(154, 20);
		this.RefinedSpeciesBox.TabIndex = 704;
		this.RefinedSpeciesBox.Visible = false;
		this.move1list.AllowDrop = true;
		this.move1list.AutoCompleteCustomSource.AddRange(new string[622]
		{
			"(None)", "Pound ", "Karate Chop ", "Double Slap ", "Comet Punch ", "Mega Punch ", "Pay Day ", "Fire Punch ", "Ice Punch ", "Thunder Punch ",
			"Scratch ", "Vice Grip ", "Guillotine ", "Razor Wind ", "Swords Dance ", "Cut ", "Gust ", "Wing Attack ", "Whirlwind ", "Fly ",
			"Bind ", "Slam ", "Vine Whip ", "Stomp ", "Double Kick ", "Mega Kick ", "Jump Kick ", "Rolling Kick ", "Sand Attack ", "Headbutt ",
			"Horn Attack ", "Fury Attack ", "Horn Drill ", "Tackle ", "Body Slam ", "Wrap ", "Take Down ", "Thrash ", "Double-Edge ", "Tail Whip ",
			"Poison Sting ", "Twineedle ", "Pin Missile ", "Leer ", "Bite ", "Growl ", "Roar ", "Sing ", "Supersonic ", "Sonic Boom ",
			"Disable ", "Acid ", "Ember ", "Flamethrower ", "Mist ", "Water Gun ", "Hydro Pump ", "Surf ", "Ice Beam ", "Blizzard ",
			"Psybeam ", "Bubble Beam ", "Aurora Beam ", "Hyper Beam ", "Peck ", "Drill Peck ", "Submission ", "Low Kick ", "Counter ", "Seismic Toss ",
			"Strength ", "Absorb ", "Mega Drain ", "Leech Seed ", "Growth ", "Razor Leaf ", "Solar Beam ", "Poison Powder ", "Stun Spore ", "Sleep Powder ",
			"Petal Dance ", "String Shot ", "Dragon Rage ", "Fire Spin ", "Thunder Shock ", "Thunderbolt ", "Thunder Wave ", "Thunder ", "Rock Throw ", "Earthquake ",
			"Fissure ", "Dig ", "Toxic ", "Confusion ", "Psychic ", "Hypnosis ", "Meditate ", "Agility ", "Quick Attack ", "Rage ",
			"Teleport ", "Night Shade ", "Mimic ", "Screech ", "Double Team ", "Recover ", "Harden ", "Minimize ", "Smokescreen ", "Confuse Ray ",
			"Withdraw ", "Defense Curl ", "Barrier ", "Light Screen ", "Haze ", "Reflect ", "Focus Energy ", "Bide ", "Metronome ", "Mirror Move ",
			"Self-Destruct ", "Egg Bomb ", "Lick ", "Smog ", "Sludge ", "Bone Club ", "Fire Blast ", "Waterfall ", "Clamp ", "Swift ",
			"Skull Bash ", "Spike Cannon ", "Constrict ", "Amnesia ", "Kinesis ", "Soft-Boiled ", "High Jump Kick ", "Glare ", "Dream Eater ", "Poison Gas ",
			"Barrage ", "Leech Life ", "Lovely Kiss ", "Sky Attack ", "Transform ", "Bubble ", "Dizzy Punch ", "Spore ", "Flash ", "Psywave ",
			"Splash ", "Acid Armor ", "Crabhammer ", "Explosion ", "Fury Swipes ", "Bonemerang ", "Rest ", "Rock Slide ", "Hyper Fang ", "Sharpen ",
			"Conversion ", "Tri Attack ", "Super Fang ", "Slash ", "Substitute ", "Struggle ", "Sketch ", "Triple Kick ", "Thief ", "Spider Web ",
			"Mind Reader ", "Nightmare ", "Flame Wheel ", "Snore ", "Curse ", "Flail ", "Conversion 2 ", "Aeroblast ", "Cotton Spore ", "Reversal ",
			"Spite ", "Powder Snow ", "Protect ", "Mach Punch ", "Scary Face ", "Feint Attack ", "Sweet Kiss ", "Belly Drum ", "Sludge Bomb ", "Mud-Slap ",
			"Octazooka ", "Spikes ", "Zap Cannon ", "Foresight ", "Destiny Bond ", "Perish Song ", "Icy Wind ", "Detect ", "Bone Rush ", "Lock-On ",
			"Outrage ", "Sandstorm ", "Giga Drain ", "Endure ", "Charm ", "Rollout ", "False Swipe ", "Swagger ", "Milk Drink ", "Spark ",
			"Fury Cutter ", "Steel Wing ", "Mean Look ", "Attract ", "Sleep Talk ", "Heal Bell ", "Return ", "Present ", "Frustration ", "Safeguard ",
			"Pain Split ", "Sacred Fire ", "Magnitude ", "Dynamic Punch ", "Megahorn ", "Dragon Breath ", "Baton Pass ", "Encore ", "Pursuit ", "Rapid Spin ",
			"Sweet Scent ", "Iron Tail ", "Metal Claw ", "Vital Throw ", "Morning Sun ", "Synthesis ", "Moonlight ", "Hidden Power ", "Cross Chop ", "Twister ",
			"Rain Dance ", "Sunny Day ", "Crunch ", "Mirror Coat ", "Psych Up ", "Extreme Speed ", "Ancient Power ", "Shadow Ball ", "Future Sight ", "Rock Smash ",
			"Whirlpool ", "Beat Up ", "Fake Out ", "Uproar ", "Stockpile ", "Spit Up ", "Swallow ", "Heat Wave ", "Hail ", "Torment ",
			"Flatter ", "Will-O-Wisp ", "Memento ", "Facade ", "Focus Punch ", "Smelling Salts ", "Follow Me ", "Nature Power ", "Charge ", "Taunt ",
			"Helping Hand ", "Trick ", "Role Play ", "Wish ", "Assist ", "Ingrain ", "Superpower ", "Magic Coat ", "Recycle ", "Revenge ",
			"Brick Break ", "Yawn ", "Knock Off ", "Endeavor ", "Eruption ", "Skill Swap ", "Imprison ", "Refresh ", "Grudge ", "Snatch ",
			"Secret Power ", "Dive ", "Arm Thrust ", "Camouflage ", "Tail Glow ", "Luster Purge ", "Mist Ball ", "Feather Dance ", "Teeter Dance ", "Blaze Kick ",
			"Mud Sport ", "Ice Ball ", "Needle Arm ", "Slack Off ", "Hyper Voice ", "Poison Fang ", "Crush Claw ", "Blast Burn ", "Hydro Cannon ", "Meteor Mash ",
			"Astonish ", "Weather Ball ", "Aromatherapy ", "Fake Tears ", "Air Cutter ", "Overheat ", "Odor Sleuth ", "Rock Tomb ", "Silver Wind ", "Metal Sound ",
			"Grass Whistle ", "Tickle ", "Cosmic Power ", "Water Spout ", "Signal Beam ", "Shadow Punch ", "Extrasensory ", "Sky Uppercut ", "Sand Tomb ", "Sheer Cold ",
			"Muddy Water ", "Bullet Seed ", "Aerial Ace ", "Icicle Spear ", "Iron Defense ", "Block ", "Howl ", "Dragon Claw ", "Frenzy Plant ", "Bulk Up ",
			"Bounce ", "Mud Shot ", "Poison Tail ", "Covet ", "Volt Tackle ", "Magical Leaf ", "Water Sport ", "Calm Mind ", "Leaf Blade ", "Dragon Dance ",
			"Rock Blast ", "Shock Wave ", "Water Pulse ", "Doom Desire ", "Psycho Boost ", "Roost ", "Gravity ", "Miracle Eye ", "Wake-Up Slap ", "Hammer Arm ",
			"Gyro Ball ", "Healing Wish ", "Brine ", "Natural Gift ", "Feint ", "Pluck ", "Tailwind ", "Acupressure ", "Metal Burst ", "U-turn ",
			"Close Combat ", "Payback ", "Assurance ", "Embargo ", "Fling ", "Psycho Shift ", "Trump Card ", "Heal Block ", "Wring Out ", "Power Trick ",
			"Gastro Acid ", "Lucky Chant ", "Me First ", "Copycat ", "Power Swap ", "Guard Swap ", "Punishment ", "Last Resort ", "Worry Seed ", "Sucker Punch ",
			"Toxic Spikes ", "Heart Swap ", "Aqua Ring ", "Magnet Rise ", "Flare Blitz ", "Force Palm ", "Aura Sphere ", "Rock Polish ", "Poison Jab ", "Dark Pulse ",
			"Night Slash ", "Aqua Tail ", "Seed Bomb ", "Air Slash ", "X-Scissor ", "Bug Buzz ", "Dragon Pulse ", "Dragon Rush ", "Power Gem ", "Drain Punch ",
			"Vacuum Wave ", "Focus Blast ", "Energy Ball ", "Brave Bird ", "Earth Power ", "Switcheroo ", "Giga Impact ", "Nasty Plot ", "Bullet Punch ", "Avalanche ",
			"Ice Shard ", "Shadow Claw ", "Thunder Fang ", "Ice Fang ", "Fire Fang ", "Shadow Sneak ", "Mud Bomb ", "Psycho Cut ", "Zen Headbutt ", "Mirror Shot ",
			"Flash Cannon ", "Rock Climb ", "Defog ", "Trick Room ", "Draco Meteor ", "Discharge ", "Lava Plume ", "Leaf Storm ", "Power Whip ", "Rock Wrecker ",
			"Cross Poison ", "Gunk Shot ", "Iron Head ", "Magnet Bomb ", "Stone Edge ", "Captivate ", "Stealth Rock ", "Grass Knot ", "Chatter ", "Judgment ",
			"Bug Bite ", "Charge Beam ", "Wood Hammer ", "Aqua Jet ", "Attack Order ", "Defend Order ", "Heal Order ", "Head Smash ", "Double Hit ", "Roar of Time ",
			"Spacial Rend ", "Lunar Dance ", "Crush Grip ", "Magma Storm ", "Dark Void ", "Seed Flare ", "Ominous Wind ", "Shadow Force ", "Hone Claws ", "Wide Guard ",
			"Guard Split ", "Power Split ", "Wonder Room ", "Psyshock ", "Venoshock ", "Autotomize ", "Rage Powder ", "Telekinesis ", "Magic Room ", "Smack Down ",
			"Storm Throw ", "Flame Burst ", "Sludge Wave ", "Quiver Dance ", "Heavy Slam ", "Synchronoise ", "Electro Ball ", "Soak ", "Flame Charge ", "Coil ",
			"Low Sweep ", "Acid Spray ", "Foul Play ", "Simple Beam ", "Entrainment ", "After You ", "Round ", "Echoed Voice ", "Chip Away ", "Clear Smog ",
			"Stored Power ", "Quick Guard ", "Ally Switch ", "Scald ", "Shell Smash ", "Heal Pulse ", "Hex ", "Sky Drop ", "Shift Gear ", "Circle Throw ",
			"Incinerate ", "Quash ", "Acrobatics ", "Reflect Type ", "Retaliate ", "Final Gambit ", "Bestow ", "Inferno ", "Water Pledge ", "Fire Pledge ",
			"Grass Pledge ", "Volt Switch ", "Struggle Bug ", "Bulldoze ", "Frost Breath ", "Dragon Tail ", "Work Up ", "Electroweb ", "Wild Charge ", "Drill Run ",
			"Dual Chop ", "Heart Stamp ", "Horn Leech ", "Sacred Sword ", "Razor Shell ", "Heat Crash ", "Leaf Tornado ", "Steamroller ", "Cotton Guard ", "Night Daze ",
			"Psystrike ", "Tail Slap ", "Hurricane ", "Head Charge ", "Gear Grind ", "Searing Shot ", "Techno Blast ", "Relic Song ", "Secret Sword ", "Glaciate ",
			"Bolt Strike ", "Blue Flare ", "Fiery Dance ", "Freeze Shock ", "Ice Burn ", "Snarl ", "Icicle Crash ", "V-create ", "Fusion Flare ", "Fusion Bolt ",
			"Flying Press ", "Mat Block ", "Belch ", "Rototiller ", "Sticky Web ", "Fell Stinger ", "Phantom Force ", "Trick-or-Treat ", "Noble Roar ", "Ion Deluge ",
			"Parabolic Charge ", "Forest's Curse ", "Petal Blizzard ", "Freeze-Dry ", "Disarming Voice ", "Parting Shot ", "Topsy-Turvy ", "Draining Kiss ", "Crafty Shield ", "Flower Shield ",
			"Grassy Terrain ", "Misty Terrain ", "Electrify ", "Play Rough ", "Fairy Wind ", "Moonblast ", "Boomburst ", "Fairy Lock ", "King's Shield ", "Play Nice ",
			"Confide ", "Diamond Storm ", "Steam Eruption ", "Hyperspace Hole ", "Water Shuriken ", "Mystical Fire ", "Spiky Shield ", "Aromatic Mist ", "Eerie Impulse ", "Venom Drench ",
			"Powder ", "Geomancy ", "Magnetic Flux ", "Happy Hour ", "Electric Terrain ", "Dazzling Gleam ", "Celebrate ", "Hold Hands ", "Baby-Doll Eyes ", "Nuzzle ",
			"Hold Back ", "Infestation ", "Power-Up Punch ", "Oblivion Wing ", "Thousand Arrows ", "Thousand Waves ", "Land's Wrath ", "Light of Ruin ", "Origin Pulse ", "Precipice Blades ",
			"Dragon Ascent ", "Hyperspace Fury "
		});
		this.move1list.FormattingEnabled = true;
		this.move1list.Items.AddRange(new object[1] { "1" });
		this.move1list.Location = new System.Drawing.Point(78, 48);
		this.move1list.Name = "move1list";
		this.move1list.Size = new System.Drawing.Size(154, 21);
		this.move1list.TabIndex = 2;
		this.Label28.AutoSize = true;
		this.Label28.Location = new System.Drawing.Point(402, 26);
		this.Label28.Name = "Label28";
		this.Label28.Size = new System.Drawing.Size(44, 13);
		this.Label28.TabIndex = 703;
		this.Label28.Text = "Form ID";
		this.Label28.Visible = false;
		this.move2list.AllowDrop = true;
		this.move2list.AutoCompleteCustomSource.AddRange(new string[622]
		{
			"(None)", "Pound ", "Karate Chop ", "Double Slap ", "Comet Punch ", "Mega Punch ", "Pay Day ", "Fire Punch ", "Ice Punch ", "Thunder Punch ",
			"Scratch ", "Vice Grip ", "Guillotine ", "Razor Wind ", "Swords Dance ", "Cut ", "Gust ", "Wing Attack ", "Whirlwind ", "Fly ",
			"Bind ", "Slam ", "Vine Whip ", "Stomp ", "Double Kick ", "Mega Kick ", "Jump Kick ", "Rolling Kick ", "Sand Attack ", "Headbutt ",
			"Horn Attack ", "Fury Attack ", "Horn Drill ", "Tackle ", "Body Slam ", "Wrap ", "Take Down ", "Thrash ", "Double-Edge ", "Tail Whip ",
			"Poison Sting ", "Twineedle ", "Pin Missile ", "Leer ", "Bite ", "Growl ", "Roar ", "Sing ", "Supersonic ", "Sonic Boom ",
			"Disable ", "Acid ", "Ember ", "Flamethrower ", "Mist ", "Water Gun ", "Hydro Pump ", "Surf ", "Ice Beam ", "Blizzard ",
			"Psybeam ", "Bubble Beam ", "Aurora Beam ", "Hyper Beam ", "Peck ", "Drill Peck ", "Submission ", "Low Kick ", "Counter ", "Seismic Toss ",
			"Strength ", "Absorb ", "Mega Drain ", "Leech Seed ", "Growth ", "Razor Leaf ", "Solar Beam ", "Poison Powder ", "Stun Spore ", "Sleep Powder ",
			"Petal Dance ", "String Shot ", "Dragon Rage ", "Fire Spin ", "Thunder Shock ", "Thunderbolt ", "Thunder Wave ", "Thunder ", "Rock Throw ", "Earthquake ",
			"Fissure ", "Dig ", "Toxic ", "Confusion ", "Psychic ", "Hypnosis ", "Meditate ", "Agility ", "Quick Attack ", "Rage ",
			"Teleport ", "Night Shade ", "Mimic ", "Screech ", "Double Team ", "Recover ", "Harden ", "Minimize ", "Smokescreen ", "Confuse Ray ",
			"Withdraw ", "Defense Curl ", "Barrier ", "Light Screen ", "Haze ", "Reflect ", "Focus Energy ", "Bide ", "Metronome ", "Mirror Move ",
			"Self-Destruct ", "Egg Bomb ", "Lick ", "Smog ", "Sludge ", "Bone Club ", "Fire Blast ", "Waterfall ", "Clamp ", "Swift ",
			"Skull Bash ", "Spike Cannon ", "Constrict ", "Amnesia ", "Kinesis ", "Soft-Boiled ", "High Jump Kick ", "Glare ", "Dream Eater ", "Poison Gas ",
			"Barrage ", "Leech Life ", "Lovely Kiss ", "Sky Attack ", "Transform ", "Bubble ", "Dizzy Punch ", "Spore ", "Flash ", "Psywave ",
			"Splash ", "Acid Armor ", "Crabhammer ", "Explosion ", "Fury Swipes ", "Bonemerang ", "Rest ", "Rock Slide ", "Hyper Fang ", "Sharpen ",
			"Conversion ", "Tri Attack ", "Super Fang ", "Slash ", "Substitute ", "Struggle ", "Sketch ", "Triple Kick ", "Thief ", "Spider Web ",
			"Mind Reader ", "Nightmare ", "Flame Wheel ", "Snore ", "Curse ", "Flail ", "Conversion 2 ", "Aeroblast ", "Cotton Spore ", "Reversal ",
			"Spite ", "Powder Snow ", "Protect ", "Mach Punch ", "Scary Face ", "Feint Attack ", "Sweet Kiss ", "Belly Drum ", "Sludge Bomb ", "Mud-Slap ",
			"Octazooka ", "Spikes ", "Zap Cannon ", "Foresight ", "Destiny Bond ", "Perish Song ", "Icy Wind ", "Detect ", "Bone Rush ", "Lock-On ",
			"Outrage ", "Sandstorm ", "Giga Drain ", "Endure ", "Charm ", "Rollout ", "False Swipe ", "Swagger ", "Milk Drink ", "Spark ",
			"Fury Cutter ", "Steel Wing ", "Mean Look ", "Attract ", "Sleep Talk ", "Heal Bell ", "Return ", "Present ", "Frustration ", "Safeguard ",
			"Pain Split ", "Sacred Fire ", "Magnitude ", "Dynamic Punch ", "Megahorn ", "Dragon Breath ", "Baton Pass ", "Encore ", "Pursuit ", "Rapid Spin ",
			"Sweet Scent ", "Iron Tail ", "Metal Claw ", "Vital Throw ", "Morning Sun ", "Synthesis ", "Moonlight ", "Hidden Power ", "Cross Chop ", "Twister ",
			"Rain Dance ", "Sunny Day ", "Crunch ", "Mirror Coat ", "Psych Up ", "Extreme Speed ", "Ancient Power ", "Shadow Ball ", "Future Sight ", "Rock Smash ",
			"Whirlpool ", "Beat Up ", "Fake Out ", "Uproar ", "Stockpile ", "Spit Up ", "Swallow ", "Heat Wave ", "Hail ", "Torment ",
			"Flatter ", "Will-O-Wisp ", "Memento ", "Facade ", "Focus Punch ", "Smelling Salts ", "Follow Me ", "Nature Power ", "Charge ", "Taunt ",
			"Helping Hand ", "Trick ", "Role Play ", "Wish ", "Assist ", "Ingrain ", "Superpower ", "Magic Coat ", "Recycle ", "Revenge ",
			"Brick Break ", "Yawn ", "Knock Off ", "Endeavor ", "Eruption ", "Skill Swap ", "Imprison ", "Refresh ", "Grudge ", "Snatch ",
			"Secret Power ", "Dive ", "Arm Thrust ", "Camouflage ", "Tail Glow ", "Luster Purge ", "Mist Ball ", "Feather Dance ", "Teeter Dance ", "Blaze Kick ",
			"Mud Sport ", "Ice Ball ", "Needle Arm ", "Slack Off ", "Hyper Voice ", "Poison Fang ", "Crush Claw ", "Blast Burn ", "Hydro Cannon ", "Meteor Mash ",
			"Astonish ", "Weather Ball ", "Aromatherapy ", "Fake Tears ", "Air Cutter ", "Overheat ", "Odor Sleuth ", "Rock Tomb ", "Silver Wind ", "Metal Sound ",
			"Grass Whistle ", "Tickle ", "Cosmic Power ", "Water Spout ", "Signal Beam ", "Shadow Punch ", "Extrasensory ", "Sky Uppercut ", "Sand Tomb ", "Sheer Cold ",
			"Muddy Water ", "Bullet Seed ", "Aerial Ace ", "Icicle Spear ", "Iron Defense ", "Block ", "Howl ", "Dragon Claw ", "Frenzy Plant ", "Bulk Up ",
			"Bounce ", "Mud Shot ", "Poison Tail ", "Covet ", "Volt Tackle ", "Magical Leaf ", "Water Sport ", "Calm Mind ", "Leaf Blade ", "Dragon Dance ",
			"Rock Blast ", "Shock Wave ", "Water Pulse ", "Doom Desire ", "Psycho Boost ", "Roost ", "Gravity ", "Miracle Eye ", "Wake-Up Slap ", "Hammer Arm ",
			"Gyro Ball ", "Healing Wish ", "Brine ", "Natural Gift ", "Feint ", "Pluck ", "Tailwind ", "Acupressure ", "Metal Burst ", "U-turn ",
			"Close Combat ", "Payback ", "Assurance ", "Embargo ", "Fling ", "Psycho Shift ", "Trump Card ", "Heal Block ", "Wring Out ", "Power Trick ",
			"Gastro Acid ", "Lucky Chant ", "Me First ", "Copycat ", "Power Swap ", "Guard Swap ", "Punishment ", "Last Resort ", "Worry Seed ", "Sucker Punch ",
			"Toxic Spikes ", "Heart Swap ", "Aqua Ring ", "Magnet Rise ", "Flare Blitz ", "Force Palm ", "Aura Sphere ", "Rock Polish ", "Poison Jab ", "Dark Pulse ",
			"Night Slash ", "Aqua Tail ", "Seed Bomb ", "Air Slash ", "X-Scissor ", "Bug Buzz ", "Dragon Pulse ", "Dragon Rush ", "Power Gem ", "Drain Punch ",
			"Vacuum Wave ", "Focus Blast ", "Energy Ball ", "Brave Bird ", "Earth Power ", "Switcheroo ", "Giga Impact ", "Nasty Plot ", "Bullet Punch ", "Avalanche ",
			"Ice Shard ", "Shadow Claw ", "Thunder Fang ", "Ice Fang ", "Fire Fang ", "Shadow Sneak ", "Mud Bomb ", "Psycho Cut ", "Zen Headbutt ", "Mirror Shot ",
			"Flash Cannon ", "Rock Climb ", "Defog ", "Trick Room ", "Draco Meteor ", "Discharge ", "Lava Plume ", "Leaf Storm ", "Power Whip ", "Rock Wrecker ",
			"Cross Poison ", "Gunk Shot ", "Iron Head ", "Magnet Bomb ", "Stone Edge ", "Captivate ", "Stealth Rock ", "Grass Knot ", "Chatter ", "Judgment ",
			"Bug Bite ", "Charge Beam ", "Wood Hammer ", "Aqua Jet ", "Attack Order ", "Defend Order ", "Heal Order ", "Head Smash ", "Double Hit ", "Roar of Time ",
			"Spacial Rend ", "Lunar Dance ", "Crush Grip ", "Magma Storm ", "Dark Void ", "Seed Flare ", "Ominous Wind ", "Shadow Force ", "Hone Claws ", "Wide Guard ",
			"Guard Split ", "Power Split ", "Wonder Room ", "Psyshock ", "Venoshock ", "Autotomize ", "Rage Powder ", "Telekinesis ", "Magic Room ", "Smack Down ",
			"Storm Throw ", "Flame Burst ", "Sludge Wave ", "Quiver Dance ", "Heavy Slam ", "Synchronoise ", "Electro Ball ", "Soak ", "Flame Charge ", "Coil ",
			"Low Sweep ", "Acid Spray ", "Foul Play ", "Simple Beam ", "Entrainment ", "After You ", "Round ", "Echoed Voice ", "Chip Away ", "Clear Smog ",
			"Stored Power ", "Quick Guard ", "Ally Switch ", "Scald ", "Shell Smash ", "Heal Pulse ", "Hex ", "Sky Drop ", "Shift Gear ", "Circle Throw ",
			"Incinerate ", "Quash ", "Acrobatics ", "Reflect Type ", "Retaliate ", "Final Gambit ", "Bestow ", "Inferno ", "Water Pledge ", "Fire Pledge ",
			"Grass Pledge ", "Volt Switch ", "Struggle Bug ", "Bulldoze ", "Frost Breath ", "Dragon Tail ", "Work Up ", "Electroweb ", "Wild Charge ", "Drill Run ",
			"Dual Chop ", "Heart Stamp ", "Horn Leech ", "Sacred Sword ", "Razor Shell ", "Heat Crash ", "Leaf Tornado ", "Steamroller ", "Cotton Guard ", "Night Daze ",
			"Psystrike ", "Tail Slap ", "Hurricane ", "Head Charge ", "Gear Grind ", "Searing Shot ", "Techno Blast ", "Relic Song ", "Secret Sword ", "Glaciate ",
			"Bolt Strike ", "Blue Flare ", "Fiery Dance ", "Freeze Shock ", "Ice Burn ", "Snarl ", "Icicle Crash ", "V-create ", "Fusion Flare ", "Fusion Bolt ",
			"Flying Press ", "Mat Block ", "Belch ", "Rototiller ", "Sticky Web ", "Fell Stinger ", "Phantom Force ", "Trick-or-Treat ", "Noble Roar ", "Ion Deluge ",
			"Parabolic Charge ", "Forest's Curse ", "Petal Blizzard ", "Freeze-Dry ", "Disarming Voice ", "Parting Shot ", "Topsy-Turvy ", "Draining Kiss ", "Crafty Shield ", "Flower Shield ",
			"Grassy Terrain ", "Misty Terrain ", "Electrify ", "Play Rough ", "Fairy Wind ", "Moonblast ", "Boomburst ", "Fairy Lock ", "King's Shield ", "Play Nice ",
			"Confide ", "Diamond Storm ", "Steam Eruption ", "Hyperspace Hole ", "Water Shuriken ", "Mystical Fire ", "Spiky Shield ", "Aromatic Mist ", "Eerie Impulse ", "Venom Drench ",
			"Powder ", "Geomancy ", "Magnetic Flux ", "Happy Hour ", "Electric Terrain ", "Dazzling Gleam ", "Celebrate ", "Hold Hands ", "Baby-Doll Eyes ", "Nuzzle ",
			"Hold Back ", "Infestation ", "Power-Up Punch ", "Oblivion Wing ", "Thousand Arrows ", "Thousand Waves ", "Land's Wrath ", "Light of Ruin ", "Origin Pulse ", "Precipice Blades ",
			"Dragon Ascent ", "Hyperspace Fury "
		});
		this.move2list.FormattingEnabled = true;
		this.move2list.Items.AddRange(new object[1] { "1" });
		this.move2list.Location = new System.Drawing.Point(78, 75);
		this.move2list.Name = "move2list";
		this.move2list.Size = new System.Drawing.Size(154, 21);
		this.move2list.TabIndex = 3;
		this.move3list.AllowDrop = true;
		this.move3list.AutoCompleteCustomSource.AddRange(new string[622]
		{
			"(None)", "Pound ", "Karate Chop ", "Double Slap ", "Comet Punch ", "Mega Punch ", "Pay Day ", "Fire Punch ", "Ice Punch ", "Thunder Punch ",
			"Scratch ", "Vice Grip ", "Guillotine ", "Razor Wind ", "Swords Dance ", "Cut ", "Gust ", "Wing Attack ", "Whirlwind ", "Fly ",
			"Bind ", "Slam ", "Vine Whip ", "Stomp ", "Double Kick ", "Mega Kick ", "Jump Kick ", "Rolling Kick ", "Sand Attack ", "Headbutt ",
			"Horn Attack ", "Fury Attack ", "Horn Drill ", "Tackle ", "Body Slam ", "Wrap ", "Take Down ", "Thrash ", "Double-Edge ", "Tail Whip ",
			"Poison Sting ", "Twineedle ", "Pin Missile ", "Leer ", "Bite ", "Growl ", "Roar ", "Sing ", "Supersonic ", "Sonic Boom ",
			"Disable ", "Acid ", "Ember ", "Flamethrower ", "Mist ", "Water Gun ", "Hydro Pump ", "Surf ", "Ice Beam ", "Blizzard ",
			"Psybeam ", "Bubble Beam ", "Aurora Beam ", "Hyper Beam ", "Peck ", "Drill Peck ", "Submission ", "Low Kick ", "Counter ", "Seismic Toss ",
			"Strength ", "Absorb ", "Mega Drain ", "Leech Seed ", "Growth ", "Razor Leaf ", "Solar Beam ", "Poison Powder ", "Stun Spore ", "Sleep Powder ",
			"Petal Dance ", "String Shot ", "Dragon Rage ", "Fire Spin ", "Thunder Shock ", "Thunderbolt ", "Thunder Wave ", "Thunder ", "Rock Throw ", "Earthquake ",
			"Fissure ", "Dig ", "Toxic ", "Confusion ", "Psychic ", "Hypnosis ", "Meditate ", "Agility ", "Quick Attack ", "Rage ",
			"Teleport ", "Night Shade ", "Mimic ", "Screech ", "Double Team ", "Recover ", "Harden ", "Minimize ", "Smokescreen ", "Confuse Ray ",
			"Withdraw ", "Defense Curl ", "Barrier ", "Light Screen ", "Haze ", "Reflect ", "Focus Energy ", "Bide ", "Metronome ", "Mirror Move ",
			"Self-Destruct ", "Egg Bomb ", "Lick ", "Smog ", "Sludge ", "Bone Club ", "Fire Blast ", "Waterfall ", "Clamp ", "Swift ",
			"Skull Bash ", "Spike Cannon ", "Constrict ", "Amnesia ", "Kinesis ", "Soft-Boiled ", "High Jump Kick ", "Glare ", "Dream Eater ", "Poison Gas ",
			"Barrage ", "Leech Life ", "Lovely Kiss ", "Sky Attack ", "Transform ", "Bubble ", "Dizzy Punch ", "Spore ", "Flash ", "Psywave ",
			"Splash ", "Acid Armor ", "Crabhammer ", "Explosion ", "Fury Swipes ", "Bonemerang ", "Rest ", "Rock Slide ", "Hyper Fang ", "Sharpen ",
			"Conversion ", "Tri Attack ", "Super Fang ", "Slash ", "Substitute ", "Struggle ", "Sketch ", "Triple Kick ", "Thief ", "Spider Web ",
			"Mind Reader ", "Nightmare ", "Flame Wheel ", "Snore ", "Curse ", "Flail ", "Conversion 2 ", "Aeroblast ", "Cotton Spore ", "Reversal ",
			"Spite ", "Powder Snow ", "Protect ", "Mach Punch ", "Scary Face ", "Feint Attack ", "Sweet Kiss ", "Belly Drum ", "Sludge Bomb ", "Mud-Slap ",
			"Octazooka ", "Spikes ", "Zap Cannon ", "Foresight ", "Destiny Bond ", "Perish Song ", "Icy Wind ", "Detect ", "Bone Rush ", "Lock-On ",
			"Outrage ", "Sandstorm ", "Giga Drain ", "Endure ", "Charm ", "Rollout ", "False Swipe ", "Swagger ", "Milk Drink ", "Spark ",
			"Fury Cutter ", "Steel Wing ", "Mean Look ", "Attract ", "Sleep Talk ", "Heal Bell ", "Return ", "Present ", "Frustration ", "Safeguard ",
			"Pain Split ", "Sacred Fire ", "Magnitude ", "Dynamic Punch ", "Megahorn ", "Dragon Breath ", "Baton Pass ", "Encore ", "Pursuit ", "Rapid Spin ",
			"Sweet Scent ", "Iron Tail ", "Metal Claw ", "Vital Throw ", "Morning Sun ", "Synthesis ", "Moonlight ", "Hidden Power ", "Cross Chop ", "Twister ",
			"Rain Dance ", "Sunny Day ", "Crunch ", "Mirror Coat ", "Psych Up ", "Extreme Speed ", "Ancient Power ", "Shadow Ball ", "Future Sight ", "Rock Smash ",
			"Whirlpool ", "Beat Up ", "Fake Out ", "Uproar ", "Stockpile ", "Spit Up ", "Swallow ", "Heat Wave ", "Hail ", "Torment ",
			"Flatter ", "Will-O-Wisp ", "Memento ", "Facade ", "Focus Punch ", "Smelling Salts ", "Follow Me ", "Nature Power ", "Charge ", "Taunt ",
			"Helping Hand ", "Trick ", "Role Play ", "Wish ", "Assist ", "Ingrain ", "Superpower ", "Magic Coat ", "Recycle ", "Revenge ",
			"Brick Break ", "Yawn ", "Knock Off ", "Endeavor ", "Eruption ", "Skill Swap ", "Imprison ", "Refresh ", "Grudge ", "Snatch ",
			"Secret Power ", "Dive ", "Arm Thrust ", "Camouflage ", "Tail Glow ", "Luster Purge ", "Mist Ball ", "Feather Dance ", "Teeter Dance ", "Blaze Kick ",
			"Mud Sport ", "Ice Ball ", "Needle Arm ", "Slack Off ", "Hyper Voice ", "Poison Fang ", "Crush Claw ", "Blast Burn ", "Hydro Cannon ", "Meteor Mash ",
			"Astonish ", "Weather Ball ", "Aromatherapy ", "Fake Tears ", "Air Cutter ", "Overheat ", "Odor Sleuth ", "Rock Tomb ", "Silver Wind ", "Metal Sound ",
			"Grass Whistle ", "Tickle ", "Cosmic Power ", "Water Spout ", "Signal Beam ", "Shadow Punch ", "Extrasensory ", "Sky Uppercut ", "Sand Tomb ", "Sheer Cold ",
			"Muddy Water ", "Bullet Seed ", "Aerial Ace ", "Icicle Spear ", "Iron Defense ", "Block ", "Howl ", "Dragon Claw ", "Frenzy Plant ", "Bulk Up ",
			"Bounce ", "Mud Shot ", "Poison Tail ", "Covet ", "Volt Tackle ", "Magical Leaf ", "Water Sport ", "Calm Mind ", "Leaf Blade ", "Dragon Dance ",
			"Rock Blast ", "Shock Wave ", "Water Pulse ", "Doom Desire ", "Psycho Boost ", "Roost ", "Gravity ", "Miracle Eye ", "Wake-Up Slap ", "Hammer Arm ",
			"Gyro Ball ", "Healing Wish ", "Brine ", "Natural Gift ", "Feint ", "Pluck ", "Tailwind ", "Acupressure ", "Metal Burst ", "U-turn ",
			"Close Combat ", "Payback ", "Assurance ", "Embargo ", "Fling ", "Psycho Shift ", "Trump Card ", "Heal Block ", "Wring Out ", "Power Trick ",
			"Gastro Acid ", "Lucky Chant ", "Me First ", "Copycat ", "Power Swap ", "Guard Swap ", "Punishment ", "Last Resort ", "Worry Seed ", "Sucker Punch ",
			"Toxic Spikes ", "Heart Swap ", "Aqua Ring ", "Magnet Rise ", "Flare Blitz ", "Force Palm ", "Aura Sphere ", "Rock Polish ", "Poison Jab ", "Dark Pulse ",
			"Night Slash ", "Aqua Tail ", "Seed Bomb ", "Air Slash ", "X-Scissor ", "Bug Buzz ", "Dragon Pulse ", "Dragon Rush ", "Power Gem ", "Drain Punch ",
			"Vacuum Wave ", "Focus Blast ", "Energy Ball ", "Brave Bird ", "Earth Power ", "Switcheroo ", "Giga Impact ", "Nasty Plot ", "Bullet Punch ", "Avalanche ",
			"Ice Shard ", "Shadow Claw ", "Thunder Fang ", "Ice Fang ", "Fire Fang ", "Shadow Sneak ", "Mud Bomb ", "Psycho Cut ", "Zen Headbutt ", "Mirror Shot ",
			"Flash Cannon ", "Rock Climb ", "Defog ", "Trick Room ", "Draco Meteor ", "Discharge ", "Lava Plume ", "Leaf Storm ", "Power Whip ", "Rock Wrecker ",
			"Cross Poison ", "Gunk Shot ", "Iron Head ", "Magnet Bomb ", "Stone Edge ", "Captivate ", "Stealth Rock ", "Grass Knot ", "Chatter ", "Judgment ",
			"Bug Bite ", "Charge Beam ", "Wood Hammer ", "Aqua Jet ", "Attack Order ", "Defend Order ", "Heal Order ", "Head Smash ", "Double Hit ", "Roar of Time ",
			"Spacial Rend ", "Lunar Dance ", "Crush Grip ", "Magma Storm ", "Dark Void ", "Seed Flare ", "Ominous Wind ", "Shadow Force ", "Hone Claws ", "Wide Guard ",
			"Guard Split ", "Power Split ", "Wonder Room ", "Psyshock ", "Venoshock ", "Autotomize ", "Rage Powder ", "Telekinesis ", "Magic Room ", "Smack Down ",
			"Storm Throw ", "Flame Burst ", "Sludge Wave ", "Quiver Dance ", "Heavy Slam ", "Synchronoise ", "Electro Ball ", "Soak ", "Flame Charge ", "Coil ",
			"Low Sweep ", "Acid Spray ", "Foul Play ", "Simple Beam ", "Entrainment ", "After You ", "Round ", "Echoed Voice ", "Chip Away ", "Clear Smog ",
			"Stored Power ", "Quick Guard ", "Ally Switch ", "Scald ", "Shell Smash ", "Heal Pulse ", "Hex ", "Sky Drop ", "Shift Gear ", "Circle Throw ",
			"Incinerate ", "Quash ", "Acrobatics ", "Reflect Type ", "Retaliate ", "Final Gambit ", "Bestow ", "Inferno ", "Water Pledge ", "Fire Pledge ",
			"Grass Pledge ", "Volt Switch ", "Struggle Bug ", "Bulldoze ", "Frost Breath ", "Dragon Tail ", "Work Up ", "Electroweb ", "Wild Charge ", "Drill Run ",
			"Dual Chop ", "Heart Stamp ", "Horn Leech ", "Sacred Sword ", "Razor Shell ", "Heat Crash ", "Leaf Tornado ", "Steamroller ", "Cotton Guard ", "Night Daze ",
			"Psystrike ", "Tail Slap ", "Hurricane ", "Head Charge ", "Gear Grind ", "Searing Shot ", "Techno Blast ", "Relic Song ", "Secret Sword ", "Glaciate ",
			"Bolt Strike ", "Blue Flare ", "Fiery Dance ", "Freeze Shock ", "Ice Burn ", "Snarl ", "Icicle Crash ", "V-create ", "Fusion Flare ", "Fusion Bolt ",
			"Flying Press ", "Mat Block ", "Belch ", "Rototiller ", "Sticky Web ", "Fell Stinger ", "Phantom Force ", "Trick-or-Treat ", "Noble Roar ", "Ion Deluge ",
			"Parabolic Charge ", "Forest's Curse ", "Petal Blizzard ", "Freeze-Dry ", "Disarming Voice ", "Parting Shot ", "Topsy-Turvy ", "Draining Kiss ", "Crafty Shield ", "Flower Shield ",
			"Grassy Terrain ", "Misty Terrain ", "Electrify ", "Play Rough ", "Fairy Wind ", "Moonblast ", "Boomburst ", "Fairy Lock ", "King's Shield ", "Play Nice ",
			"Confide ", "Diamond Storm ", "Steam Eruption ", "Hyperspace Hole ", "Water Shuriken ", "Mystical Fire ", "Spiky Shield ", "Aromatic Mist ", "Eerie Impulse ", "Venom Drench ",
			"Powder ", "Geomancy ", "Magnetic Flux ", "Happy Hour ", "Electric Terrain ", "Dazzling Gleam ", "Celebrate ", "Hold Hands ", "Baby-Doll Eyes ", "Nuzzle ",
			"Hold Back ", "Infestation ", "Power-Up Punch ", "Oblivion Wing ", "Thousand Arrows ", "Thousand Waves ", "Land's Wrath ", "Light of Ruin ", "Origin Pulse ", "Precipice Blades ",
			"Dragon Ascent ", "Hyperspace Fury "
		});
		this.move3list.FormattingEnabled = true;
		this.move3list.Items.AddRange(new object[1] { "1" });
		this.move3list.Location = new System.Drawing.Point(310, 48);
		this.move3list.Name = "move3list";
		this.move3list.Size = new System.Drawing.Size(154, 21);
		this.move3list.TabIndex = 4;
		this.move4list.AllowDrop = true;
		this.move4list.AutoCompleteCustomSource.AddRange(new string[622]
		{
			"(None)", "Pound ", "Karate Chop ", "Double Slap ", "Comet Punch ", "Mega Punch ", "Pay Day ", "Fire Punch ", "Ice Punch ", "Thunder Punch ",
			"Scratch ", "Vice Grip ", "Guillotine ", "Razor Wind ", "Swords Dance ", "Cut ", "Gust ", "Wing Attack ", "Whirlwind ", "Fly ",
			"Bind ", "Slam ", "Vine Whip ", "Stomp ", "Double Kick ", "Mega Kick ", "Jump Kick ", "Rolling Kick ", "Sand Attack ", "Headbutt ",
			"Horn Attack ", "Fury Attack ", "Horn Drill ", "Tackle ", "Body Slam ", "Wrap ", "Take Down ", "Thrash ", "Double-Edge ", "Tail Whip ",
			"Poison Sting ", "Twineedle ", "Pin Missile ", "Leer ", "Bite ", "Growl ", "Roar ", "Sing ", "Supersonic ", "Sonic Boom ",
			"Disable ", "Acid ", "Ember ", "Flamethrower ", "Mist ", "Water Gun ", "Hydro Pump ", "Surf ", "Ice Beam ", "Blizzard ",
			"Psybeam ", "Bubble Beam ", "Aurora Beam ", "Hyper Beam ", "Peck ", "Drill Peck ", "Submission ", "Low Kick ", "Counter ", "Seismic Toss ",
			"Strength ", "Absorb ", "Mega Drain ", "Leech Seed ", "Growth ", "Razor Leaf ", "Solar Beam ", "Poison Powder ", "Stun Spore ", "Sleep Powder ",
			"Petal Dance ", "String Shot ", "Dragon Rage ", "Fire Spin ", "Thunder Shock ", "Thunderbolt ", "Thunder Wave ", "Thunder ", "Rock Throw ", "Earthquake ",
			"Fissure ", "Dig ", "Toxic ", "Confusion ", "Psychic ", "Hypnosis ", "Meditate ", "Agility ", "Quick Attack ", "Rage ",
			"Teleport ", "Night Shade ", "Mimic ", "Screech ", "Double Team ", "Recover ", "Harden ", "Minimize ", "Smokescreen ", "Confuse Ray ",
			"Withdraw ", "Defense Curl ", "Barrier ", "Light Screen ", "Haze ", "Reflect ", "Focus Energy ", "Bide ", "Metronome ", "Mirror Move ",
			"Self-Destruct ", "Egg Bomb ", "Lick ", "Smog ", "Sludge ", "Bone Club ", "Fire Blast ", "Waterfall ", "Clamp ", "Swift ",
			"Skull Bash ", "Spike Cannon ", "Constrict ", "Amnesia ", "Kinesis ", "Soft-Boiled ", "High Jump Kick ", "Glare ", "Dream Eater ", "Poison Gas ",
			"Barrage ", "Leech Life ", "Lovely Kiss ", "Sky Attack ", "Transform ", "Bubble ", "Dizzy Punch ", "Spore ", "Flash ", "Psywave ",
			"Splash ", "Acid Armor ", "Crabhammer ", "Explosion ", "Fury Swipes ", "Bonemerang ", "Rest ", "Rock Slide ", "Hyper Fang ", "Sharpen ",
			"Conversion ", "Tri Attack ", "Super Fang ", "Slash ", "Substitute ", "Struggle ", "Sketch ", "Triple Kick ", "Thief ", "Spider Web ",
			"Mind Reader ", "Nightmare ", "Flame Wheel ", "Snore ", "Curse ", "Flail ", "Conversion 2 ", "Aeroblast ", "Cotton Spore ", "Reversal ",
			"Spite ", "Powder Snow ", "Protect ", "Mach Punch ", "Scary Face ", "Feint Attack ", "Sweet Kiss ", "Belly Drum ", "Sludge Bomb ", "Mud-Slap ",
			"Octazooka ", "Spikes ", "Zap Cannon ", "Foresight ", "Destiny Bond ", "Perish Song ", "Icy Wind ", "Detect ", "Bone Rush ", "Lock-On ",
			"Outrage ", "Sandstorm ", "Giga Drain ", "Endure ", "Charm ", "Rollout ", "False Swipe ", "Swagger ", "Milk Drink ", "Spark ",
			"Fury Cutter ", "Steel Wing ", "Mean Look ", "Attract ", "Sleep Talk ", "Heal Bell ", "Return ", "Present ", "Frustration ", "Safeguard ",
			"Pain Split ", "Sacred Fire ", "Magnitude ", "Dynamic Punch ", "Megahorn ", "Dragon Breath ", "Baton Pass ", "Encore ", "Pursuit ", "Rapid Spin ",
			"Sweet Scent ", "Iron Tail ", "Metal Claw ", "Vital Throw ", "Morning Sun ", "Synthesis ", "Moonlight ", "Hidden Power ", "Cross Chop ", "Twister ",
			"Rain Dance ", "Sunny Day ", "Crunch ", "Mirror Coat ", "Psych Up ", "Extreme Speed ", "Ancient Power ", "Shadow Ball ", "Future Sight ", "Rock Smash ",
			"Whirlpool ", "Beat Up ", "Fake Out ", "Uproar ", "Stockpile ", "Spit Up ", "Swallow ", "Heat Wave ", "Hail ", "Torment ",
			"Flatter ", "Will-O-Wisp ", "Memento ", "Facade ", "Focus Punch ", "Smelling Salts ", "Follow Me ", "Nature Power ", "Charge ", "Taunt ",
			"Helping Hand ", "Trick ", "Role Play ", "Wish ", "Assist ", "Ingrain ", "Superpower ", "Magic Coat ", "Recycle ", "Revenge ",
			"Brick Break ", "Yawn ", "Knock Off ", "Endeavor ", "Eruption ", "Skill Swap ", "Imprison ", "Refresh ", "Grudge ", "Snatch ",
			"Secret Power ", "Dive ", "Arm Thrust ", "Camouflage ", "Tail Glow ", "Luster Purge ", "Mist Ball ", "Feather Dance ", "Teeter Dance ", "Blaze Kick ",
			"Mud Sport ", "Ice Ball ", "Needle Arm ", "Slack Off ", "Hyper Voice ", "Poison Fang ", "Crush Claw ", "Blast Burn ", "Hydro Cannon ", "Meteor Mash ",
			"Astonish ", "Weather Ball ", "Aromatherapy ", "Fake Tears ", "Air Cutter ", "Overheat ", "Odor Sleuth ", "Rock Tomb ", "Silver Wind ", "Metal Sound ",
			"Grass Whistle ", "Tickle ", "Cosmic Power ", "Water Spout ", "Signal Beam ", "Shadow Punch ", "Extrasensory ", "Sky Uppercut ", "Sand Tomb ", "Sheer Cold ",
			"Muddy Water ", "Bullet Seed ", "Aerial Ace ", "Icicle Spear ", "Iron Defense ", "Block ", "Howl ", "Dragon Claw ", "Frenzy Plant ", "Bulk Up ",
			"Bounce ", "Mud Shot ", "Poison Tail ", "Covet ", "Volt Tackle ", "Magical Leaf ", "Water Sport ", "Calm Mind ", "Leaf Blade ", "Dragon Dance ",
			"Rock Blast ", "Shock Wave ", "Water Pulse ", "Doom Desire ", "Psycho Boost ", "Roost ", "Gravity ", "Miracle Eye ", "Wake-Up Slap ", "Hammer Arm ",
			"Gyro Ball ", "Healing Wish ", "Brine ", "Natural Gift ", "Feint ", "Pluck ", "Tailwind ", "Acupressure ", "Metal Burst ", "U-turn ",
			"Close Combat ", "Payback ", "Assurance ", "Embargo ", "Fling ", "Psycho Shift ", "Trump Card ", "Heal Block ", "Wring Out ", "Power Trick ",
			"Gastro Acid ", "Lucky Chant ", "Me First ", "Copycat ", "Power Swap ", "Guard Swap ", "Punishment ", "Last Resort ", "Worry Seed ", "Sucker Punch ",
			"Toxic Spikes ", "Heart Swap ", "Aqua Ring ", "Magnet Rise ", "Flare Blitz ", "Force Palm ", "Aura Sphere ", "Rock Polish ", "Poison Jab ", "Dark Pulse ",
			"Night Slash ", "Aqua Tail ", "Seed Bomb ", "Air Slash ", "X-Scissor ", "Bug Buzz ", "Dragon Pulse ", "Dragon Rush ", "Power Gem ", "Drain Punch ",
			"Vacuum Wave ", "Focus Blast ", "Energy Ball ", "Brave Bird ", "Earth Power ", "Switcheroo ", "Giga Impact ", "Nasty Plot ", "Bullet Punch ", "Avalanche ",
			"Ice Shard ", "Shadow Claw ", "Thunder Fang ", "Ice Fang ", "Fire Fang ", "Shadow Sneak ", "Mud Bomb ", "Psycho Cut ", "Zen Headbutt ", "Mirror Shot ",
			"Flash Cannon ", "Rock Climb ", "Defog ", "Trick Room ", "Draco Meteor ", "Discharge ", "Lava Plume ", "Leaf Storm ", "Power Whip ", "Rock Wrecker ",
			"Cross Poison ", "Gunk Shot ", "Iron Head ", "Magnet Bomb ", "Stone Edge ", "Captivate ", "Stealth Rock ", "Grass Knot ", "Chatter ", "Judgment ",
			"Bug Bite ", "Charge Beam ", "Wood Hammer ", "Aqua Jet ", "Attack Order ", "Defend Order ", "Heal Order ", "Head Smash ", "Double Hit ", "Roar of Time ",
			"Spacial Rend ", "Lunar Dance ", "Crush Grip ", "Magma Storm ", "Dark Void ", "Seed Flare ", "Ominous Wind ", "Shadow Force ", "Hone Claws ", "Wide Guard ",
			"Guard Split ", "Power Split ", "Wonder Room ", "Psyshock ", "Venoshock ", "Autotomize ", "Rage Powder ", "Telekinesis ", "Magic Room ", "Smack Down ",
			"Storm Throw ", "Flame Burst ", "Sludge Wave ", "Quiver Dance ", "Heavy Slam ", "Synchronoise ", "Electro Ball ", "Soak ", "Flame Charge ", "Coil ",
			"Low Sweep ", "Acid Spray ", "Foul Play ", "Simple Beam ", "Entrainment ", "After You ", "Round ", "Echoed Voice ", "Chip Away ", "Clear Smog ",
			"Stored Power ", "Quick Guard ", "Ally Switch ", "Scald ", "Shell Smash ", "Heal Pulse ", "Hex ", "Sky Drop ", "Shift Gear ", "Circle Throw ",
			"Incinerate ", "Quash ", "Acrobatics ", "Reflect Type ", "Retaliate ", "Final Gambit ", "Bestow ", "Inferno ", "Water Pledge ", "Fire Pledge ",
			"Grass Pledge ", "Volt Switch ", "Struggle Bug ", "Bulldoze ", "Frost Breath ", "Dragon Tail ", "Work Up ", "Electroweb ", "Wild Charge ", "Drill Run ",
			"Dual Chop ", "Heart Stamp ", "Horn Leech ", "Sacred Sword ", "Razor Shell ", "Heat Crash ", "Leaf Tornado ", "Steamroller ", "Cotton Guard ", "Night Daze ",
			"Psystrike ", "Tail Slap ", "Hurricane ", "Head Charge ", "Gear Grind ", "Searing Shot ", "Techno Blast ", "Relic Song ", "Secret Sword ", "Glaciate ",
			"Bolt Strike ", "Blue Flare ", "Fiery Dance ", "Freeze Shock ", "Ice Burn ", "Snarl ", "Icicle Crash ", "V-create ", "Fusion Flare ", "Fusion Bolt ",
			"Flying Press ", "Mat Block ", "Belch ", "Rototiller ", "Sticky Web ", "Fell Stinger ", "Phantom Force ", "Trick-or-Treat ", "Noble Roar ", "Ion Deluge ",
			"Parabolic Charge ", "Forest's Curse ", "Petal Blizzard ", "Freeze-Dry ", "Disarming Voice ", "Parting Shot ", "Topsy-Turvy ", "Draining Kiss ", "Crafty Shield ", "Flower Shield ",
			"Grassy Terrain ", "Misty Terrain ", "Electrify ", "Play Rough ", "Fairy Wind ", "Moonblast ", "Boomburst ", "Fairy Lock ", "King's Shield ", "Play Nice ",
			"Confide ", "Diamond Storm ", "Steam Eruption ", "Hyperspace Hole ", "Water Shuriken ", "Mystical Fire ", "Spiky Shield ", "Aromatic Mist ", "Eerie Impulse ", "Venom Drench ",
			"Powder ", "Geomancy ", "Magnetic Flux ", "Happy Hour ", "Electric Terrain ", "Dazzling Gleam ", "Celebrate ", "Hold Hands ", "Baby-Doll Eyes ", "Nuzzle ",
			"Hold Back ", "Infestation ", "Power-Up Punch ", "Oblivion Wing ", "Thousand Arrows ", "Thousand Waves ", "Land's Wrath ", "Light of Ruin ", "Origin Pulse ", "Precipice Blades ",
			"Dragon Ascent ", "Hyperspace Fury "
		});
		this.move4list.FormattingEnabled = true;
		this.move4list.Items.AddRange(new object[1] { "1" });
		this.move4list.Location = new System.Drawing.Point(310, 75);
		this.move4list.Name = "move4list";
		this.move4list.Size = new System.Drawing.Size(154, 21);
		this.move4list.TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(12, 51);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(43, 13);
		this.label11.TabIndex = 680;
		this.label11.Text = "Move 1";
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(12, 78);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(43, 13);
		this.label10.TabIndex = 681;
		this.label10.Text = "Move 2";
		this.Label4.AutoSize = true;
		this.Label4.Location = new System.Drawing.Point(250, 51);
		this.Label4.Name = "Label4";
		this.Label4.Size = new System.Drawing.Size(43, 13);
		this.Label4.TabIndex = 682;
		this.Label4.Text = "Move 3";
		this.pokedextype.AllowDrop = true;
		this.pokedextype.FormattingEnabled = true;
		this.pokedextype.Items.AddRange(new object[900]
		{
			"????? Pokémon", "Seed Pokémon", "Seed Pokémon", "Seed Pokémon", "Lizard Pokémon", "Flame Pokémon", "Flame Pokémon", "Tiny Turtle Pokémon", "Turtle Pokémon", "Shellfish Pokémon",
			"Worm Pokémon", "Cocoon Pokémon", "Butterfly Pokémon", "Hairy Bug Pokémon", "Cocoon Pokémon", "Poison Bee Pokémon", "Tiny Bird Pokémon", "Bird Pokémon", "Bird Pokémon", "Mouse Pokémon",
			"Mouse Pokémon", "Tiny Bird Pokémon", "Beak Pokémon", "Snake Pokémon", "Cobra Pokémon", "Mouse Pokémon", "Mouse Pokémon", "Mouse Pokémon", "Mouse Pokémon", "Poison Pin Pokémon",
			"Poison Pin Pokémon", "Drill Pokémon", "Poison Pin Pokémon", "Poison Pin Pokémon", "Drill Pokémon", "Fairy Pokémon", "Fairy Pokémon", "Fox Pokémon", "Fox Pokémon", "Balloon Pokémon",
			"Balloon Pokémon", "Bat Pokémon", "Bat Pokémon", "Weed Pokémon", "Weed Pokémon", "Flower Pokémon", "Mushroom Pokémon", "Mushroom Pokémon", "Insect Pokémon", "Poison Moth Pokémon",
			"Mole Pokémon", "Mole Pokémon", "Scratch Cat Pokémon", "Classy Cat Pokémon", "Duck Pokémon", "Duck Pokémon", "Pig Monkey Pokémon", "Pig Monkey Pokémon", "Puppy Pokémon", "Legendary Pokémon",
			"Tadpole Pokémon", "Tadpole Pokémon", "Tadpole Pokémon", "Psi Pokémon", "Psi Pokémon", "Psi Pokémon", "Superpower Pokémon", "Superpower Pokémon", "Superpower Pokémon", "Flower Pokémon",
			"Flycatcher Pokémon", "Flycatcher Pokémon", "Jellyfish Pokémon", "Jellyfish Pokémon", "Rock Pokémon", "Rock Pokémon", "Megaton Pokémon", "Fire Horse Pokémon", "Fire Horse Pokémon", "Dopey Pokémon",
			"Hermit Crab Pokémon", "Magnet Pokémon", "Magnet Pokémon", "Wild Duck Pokémon", "Twin Bird Pokémon", "Triple Bird Pokémon", "Sea Lion Pokémon", "Sea Lion Pokémon", "Sludge Pokémon", "Sludge Pokémon",
			"Bivalve Pokémon", "Bivalve Pokémon", "Gas Pokémon", "Gas Pokémon", "Shadow Pokémon", "Rock Snake Pokémon", "Hypnosis Pokémon", "Hypnosis Pokémon", "River Crab Pokémon", "Pincer Pokémon",
			"Ball Pokémon", "Ball Pokémon", "Egg Pokémon", "Coconut Pokémon", "Lonely Pokémon", "Bone Keeper Pokémon", "Kicking Pokémon", "Punching Pokémon", "Licking Pokémon", "Poison Gas Pokémon",
			"Poison Gas Pokémon", "Spikes Pokémon", "Drill Pokémon", "Egg Pokémon", "Vine Pokémon", "Parent Pokémon", "Dragon Pokémon", "Dragon Pokémon", "Goldfish Pokémon", "Goldfish Pokémon",
			"Star Shape Pokémon", "Mysterious Pokémon", "Barrier Pokémon", "Mantis Pokémon", "Human Shape Pokémon", "Electric Pokémon", "Spitfire Pokémon", "Stag Beetle Pokémon", "Wild Bull Pokémon", "Fish Pokémon",
			"Atrocious Pokémon", "Transport Pokémon", "Transform Pokémon", "Evolution Pokémon", "Bubble Jet Pokémon", "Lightning Pokémon", "Flame Pokémon", "Virtual Pokémon", "Spiral Pokémon", "Spiral Pokémon",
			"Shellfish Pokémon", "Shellfish Pokémon", "Fossil Pokémon", "Sleeping Pokémon", "Freeze Pokémon", "Electric Pokémon", "Flame Pokémon", "Dragon Pokémon", "Dragon Pokémon", "Dragon Pokémon",
			"Genetic Pokémon", "New Species Pokémon", "Leaf Pokémon", "Leaf Pokémon", "Herb Pokémon", "Fire Mouse Pokémon", "Volcano Pokémon", "Volcano Pokémon", "Big Jaw Pokémon", "Big Jaw Pokémon",
			"Big Jaw Pokémon", "Scout Pokémon", "Long Body Pokémon", "Owl Pokémon", "Owl Pokémon", "Five Star Pokémon", "Five Star Pokémon", "String Spit Pokémon", "Long Leg Pokémon", "Bat Pokémon",
			"Angler Pokémon", "Light Pokémon", "Tiny Mouse Pokémon", "Star Shape Pokémon", "Balloon Pokémon", "Spike Ball Pokémon", "Happiness Pokémon", "Tiny Bird Pokémon", "Mystic Pokémon", "Wool Pokémon",
			"Wool Pokémon", "Light Pokémon", "Flower Pokémon", "Aqua Mouse Pokémon", "Aqua Rabbit Pokémon", "Imitation Pokémon", "Frog Pokémon", "Cottonweed Pokémon", "Cottonweed Pokémon", "Cottonweed Pokémon",
			"Long Tail Pokémon", "Seed Pokémon", "Sun Pokémon", "Clear Wing Pokémon", "Water Fish Pokémon", "Water Fish Pokémon", "Sun Pokémon", "Moonlight Pokémon", "Darkness Pokémon", "Royal Pokémon",
			"Screech Pokémon", "Symbol Pokémon", "Patient Pokémon", "Long Neck Pokémon", "Bagworm Pokémon", "Bagworm Pokémon", "Land Snake Pokémon", "Fly Scorpion Pokémon", "Iron Snake Pokémon", "Fairy Pokémon",
			"Fairy Pokémon", "Balloon Pokémon", "Pincer Pokémon", "Mold Pokémon", "Single Horn Pokémon", "Sharp Claw Pokémon", "Little Bear Pokémon", "Hibernator Pokémon", "Lava Pokémon", "Lava Pokémon",
			"Pig Pokémon", "Swine Pokémon", "Coral Pokémon", "Jet Pokémon", "Jet Pokémon", "Delivery Pokémon", "Kite Pokémon", "Armor Bird Pokémon", "Dark Pokémon", "Dark Pokémon",
			"Dragon Pokémon", "Long Nose Pokémon", "Armor Pokémon", "Virtual Pokémon", "Big Horn Pokémon", "Painter Pokémon", "Scuffle Pokémon", "Handstand Pokémon", "Kiss Pokémon", "Electric Pokémon",
			"Live Coal Pokémon", "Milk Cow Pokémon", "Happiness Pokémon", "Thunder Pokémon", "Volcano Pokémon", "Aurora Pokémon", "Rock Skin Pokémon", "Hard Shell Pokémon", "Armor Pokémon", "Diving Pokémon",
			"Rainbow Pokémon", "Time Travel Pokémon", "Wood Gecko Pokémon", "Wood Gecko Pokémon", "Forest Pokémon", "Chick Pokémon", "Young Fowl Pokémon", "Blaze Pokémon", "Mud Fish Pokémon", "Mud Fish Pokémon",
			"Mud Fish Pokémon", "Bite Pokémon", "Bite Pokémon", "Tiny Raccoon Pokémon", "Rushing Pokémon", "Worm Pokémon", "Cocoon Pokémon", "Butterfly Pokémon", "Cocoon Pokémon", "Poison Moth Pokémon",
			"Water Weed Pokémon", "Jolly Pokémon", "Carefree Pokémon", "Acorn Pokémon", "Wily Pokémon", "Wicked Pokémon", "Tiny Swallow Pokémon", "Swallow Pokémon", "Seagull Pokémon", "Water Bird Pokémon",
			"Feeling Pokémon", "Emotion Pokémon", "Embrace Pokémon", "Pond Skater Pokémon", "Eyeball Pokémon", "Mushroom Pokémon", "Mushroom Pokémon", "Slacker Pokémon", "Wild Monkey Pokémon", "Lazy Pokémon",
			"Trainee Pokémon", "Ninja Pokémon", "Shed Pokémon", "Whisper Pokémon", "Big Voice Pokémon", "Loud Noise Pokémon", "Guts Pokémon", "Arm Thrust Pokémon", "Polka Dot Pokémon", "Compass Pokémon",
			"Kitten Pokémon", "Prim Pokémon", "Darkness Pokémon", "Deceiver Pokémon", "Iron Armor Pokémon", "Iron Armor Pokémon", "Iron Armor Pokémon", "Meditate Pokémon", "Meditate Pokémon", "Lightning Pokémon",
			"Discharge Pokémon", "Cheering Pokémon", "Cheering Pokémon", "Firefly Pokémon", "Firefly Pokémon", "Thorn Pokémon", "Stomach Pokémon", "Poison Bag Pokémon", "Savage Pokémon", "Brutal Pokémon",
			"Ball Whale Pokémon", "Float Whale Pokémon", "Numb Pokémon", "Eruption Pokémon", "Coal Pokémon", "Bounce Pokémon", "Manipulate Pokémon", "Spot Panda Pokémon", "Ant Pit Pokémon", "Vibration Pokémon",
			"Mystic Pokémon", "Cactus Pokémon", "Scarecrow Pokémon", "Cotton Bird Pokémon", "Humming Pokémon", "Cat Ferret Pokémon", "Fang Snake Pokémon", "Meteorite Pokémon", "Meteorite Pokémon", "Whiskers Pokémon",
			"Whiskers Pokémon", "Ruffian Pokémon", "Rogue Pokémon", "Clay Doll Pokémon", "Clay Doll Pokémon", "Sea Lily Pokémon", "Barnacle Pokémon", "Old Shrimp Pokémon", "Plate Pokémon", "Fish Pokémon",
			"Tender Pokémon", "Weather Pokémon", "Color Swap Pokémon", "Puppet Pokémon", "Marionette Pokémon", "Requiem Pokémon", "Beckon Pokémon", "Fruit Pokémon", "Wind Chime Pokémon", "Disaster Pokémon",
			"Bright Pokémon", "Snow Hat Pokémon", "Face Pokémon", "Clap Pokémon", "Ball Roll Pokémon", "Ice Break Pokémon", "Bivalve Pokémon", "Deep Sea Pokémon", "South Sea Pokémon", "Longevity Pokémon",
			"Rendezvous Pokémon", "Rock Head Pokémon", "Endurance Pokémon", "Dragon Pokémon", "Iron Ball Pokémon", "Iron Claw Pokémon", "Iron Leg Pokémon", "Rock Peak Pokémon", "Iceberg Pokémon", "Iron Pokémon",
			"Eon Pokémon", "Eon Pokémon", "Sea Basin Pokémon", "Continent Pokémon", "Sky High Pokémon", "Wish Pokémon", "DNA Pokémon", "Tiny Leaf Pokémon", "Grove Pokémon", "Continent Pokémon",
			"Chimp Pokémon", "Playful Pokémon", "Flame Pokémon", "Penguin Pokémon", "Penguin Pokémon", "Emperor Pokémon", "Starling Pokémon", "Starling Pokémon", "Predator Pokémon", "Plump Mouse Pokémon",
			"Beaver Pokémon", "Cricket Pokémon", "Cricket Pokémon", "Flash Pokémon", "Spark Pokémon", "Gleam Eyes Pokémon", "Bud Pokémon", "Bouquet Pokémon", "Head Butt Pokémon", "Head Butt Pokémon",
			"Shield Pokémon", "Shield Pokémon", "Bagworm Pokémon", "Bagworm Pokémon", "Moth Pokémon", "Tiny Bee Pokémon", "Beehive Pokémon", "EleSquirrel Pokémon", "Sea Weasel Pokémon", "Sea Weasel Pokémon",
			"Cherry Pokémon", "Blossom Pokémon", "Sea Slug Pokémon", "Sea Slug Pokémon", "Long Tail Pokémon", "Balloon Pokémon", "Blimp Pokémon", "Rabbit Pokémon", "Rabbit Pokémon", "Magical Pokémon",
			"Big Boss Pokémon", "Catty Pokémon", "Tiger Cat Pokémon", "Bell Pokémon", "Skunk Pokémon", "Skunk Pokémon", "Bronze Pokémon", "Bronze Bell Pokémon", "Bonsai Pokémon", "Mime Pokémon",
			"Playhouse Pokémon", "Music Note Pokémon", "Forbidden Pokémon", "Land Shark Pokémon", "Cave Pokémon", "Mach Pokémon", "Big Eater Pokémon", "Emanation Pokémon", "Aura Pokémon", "Hippo Pokémon",
			"Heavyweight Pokémon", "Scorpion Pokémon", "Ogre Scorpion Pokémon", "Toxic Mouth Pokémon", "Toxic Mouth Pokémon", "Bug Catcher Pokémon", "Wing Fish Pokémon", "Neon Pokémon", "Kite Pokémon", "Frost Tree Pokémon",
			"Frost Tree Pokémon", "Sharp Claw Pokémon", "Magnet Area Pokémon", "Licking Pokémon", "Drill Pokémon", "Vine Pokémon", "Thunderbolt Pokémon", "Blast Pokémon", "Jubilee Pokémon", "Ogre Darner Pokémon",
			"Verdant Pokémon", "Fresh Snow Pokémon", "Fang Scorpion Pokémon", "Twin Tusk Pokémon", "Virtual Pokémon", "Blade Pokémon", "Compass Pokémon", "Gripper Pokémon", "Snow Land Pokémon", "Plasma Pokémon",
			"Knowledge Pokémon", "Emotion Pokémon", "Willpower Pokémon", "Temporal Pokémon", "Spatial Pokémon", "Lava Dome Pokémon", "Colossal Pokémon", "Renegade Pokémon", "Lunar Pokémon", "Sea Drifter Pokémon",
			"Seafaring Pokémon", "Pitch-Black Pokémon", "Gratitude Pokémon", "Alpha Pokémon", "Victory Pokémon", "Grass Snake Pokémon", "Grass Snake Pokémon", "Regal Pokémon", "Fire Pig Pokémon", "Fire Pig Pokémon",
			"Mega Fire Pig Pokémon", "Sea Otter Pokémon", "Discipline Pokémon", "Formidable Pokémon", "Scout Pokémon", "Lookout Pokémon", "Puppy Pokémon", "Loyal Dog Pokémon", "Big-Hearted Pokémon", "Devious Pokémon",
			"Cruel Pokémon", "Grass Monkey Pokémon", "Thorn Monkey Pokémon", "High Temp Pokémon", "Ember Pokémon", "Spray Pokémon", "Geyser Pokémon", "Dream Eater Pokémon", "Drowsing Pokémon", "Tiny Pigeon Pokémon",
			"Wild Pigeon Pokémon", "Proud Pokémon", "Electrified Pokémon", "Thunderbolt Pokémon", "Mantle Pokémon", "Ore Pokémon", "Compressed Pokémon", "Bat Pokémon", "Courting Pokémon", "Mole Pokémon",
			"Subterrene Pokémon", "Hearing Pokémon", "Muscular Pokémon", "Muscular Pokémon", "Muscular Pokémon", "Tadpole Pokémon", "Vibration Pokémon", "Vibration Pokémon", "Judo Pokémon", "Karate Pokémon",
			"Sewing Pokémon", "Leaf-Wrapped Pokémon", "Nurturing Pokémon", "Centipede Pokémon", "Curlipede Pokémon", "Megapede Pokémon", "Cotton Puff Pokémon", "Windveiled Pokémon", "Bulb Pokémon", "Flowering Pokémon",
			"Hostile Pokémon", "Desert Croc Pokémon", "Desert Croc Pokémon", "Intimidation Pokémon", "Zen Charm Pokémon", "Blazing Pokémon", "Cactus Pokémon", "Rock Inn Pokémon", "Stone Home Pokémon", "Shedding Pokémon",
			"Hoodlum Pokémon", "Avianoid Pokémon", "Spirit Pokémon", "Coffin Pokémon", "Prototurtle Pokémon", "Prototurtle Pokémon", "First Bird Pokémon", "First Bird Pokémon", "Trash Bag Pokémon", "Trash Heap Pokémon",
			"Tricky Fox Pokémon", "Illusion Fox Pokémon", "Chinchilla Pokémon", "Scarf Pokémon", "Fixation Pokémon", "Manipulate Pokémon", "Astral Body Pokémon", "Cell Pokémon", "Mitosis Pokémon", "Multiplying Pokémon",
			"Water Bird Pokémon", "White Bird Pokémon", "Fresh Snow Pokémon", "Icy Snow Pokémon", "Snowstorm Pokémon", "Season Pokémon", "Season Pokémon", "Sky Squirrel Pokémon", "Clamping Pokémon", "Cavalry Pokémon",
			"Mushroom Pokémon", "Mushroom Pokémon", "Floating Pokémon", "Floating Pokémon", "Caring Pokémon", "Attaching Pokémon", "EleSpider Pokémon", "Thorn Seed Pokémon", "Thorn Pod Pokémon", "Gear Pokémon",
			"Gear Pokémon", "Gear Pokémon", "EleFish Pokémon", "EleFish Pokémon", "EleFish Pokémon", "Cerebral Pokémon", "Cerebral Pokémon", "Candle Pokémon", "Lamp Pokémon", "Luring Pokémon",
			"Tusk Pokémon", "Axe Jaw Pokémon", "Axe Jaw Pokémon", "Chill Pokémon", "Freezing Pokémon", "Crystallizing Pokémon", "Snail Pokémon", "Shell Out Pokémon", "Trap Pokémon", "Martial Arts Pokémon",
			"Martial Arts Pokémon", "Cave Pokémon", "Automaton Pokémon", "Automaton Pokémon", "Sharp Blade Pokémon", "Sword Blade Pokémon", "Bash Buffalo Pokémon", "Eaglet Pokémon", "Valiant Pokémon", "Diapered Pokémon",
			"Bone Vulture Pokémon", "Anteater Pokémon", "Iron Ant Pokémon", "Irate Pokémon", "Hostile Pokémon", "Brutal Pokémon", "Torch Pokémon", "Sun Pokémon", "Iron Will Pokémon", "Cavern Pokémon",
			"Grassland Pokémon", "Cyclone Pokémon", "Bolt Strike Pokémon", "Vast White Pokémon", "Deep Black Pokémon", "Abundance Pokémon", "Boundary Pokémon", "Colt Pokémon", "Melody Pokémon", "Paleozoic Pokémon",
			"Spiny Nut Pokémon", "Spiny Armor Pokémon", "Spiny Armor Pokémon", "Fox Pokémon", "Fox Pokémon", "Fox Pokémon", "Bubble Frog Pokémon", "Bubble Frog Pokémon", "Ninja Pokémon", "Digging Pokémon",
			"Digging Pokémon", "Tiny Robin Pokémon", "Ember Pokémon", "Scorching Pokémon", "Scatterdust Pokémon", "Scatterdust Pokémon", "Scale Pokémon", "Lion Cub Pokémon", "Royal Pokémon", "Single Bloom Pokémon",
			"Single Bloom Pokémon", "Garden Pokémon", "Mount Pokémon", "Mount Pokémon", "Playful Pokémon", "Daunting Pokémon", "Poodle Pokémon", "Restraint Pokémon", "Constraint Pokémon", "Sword Pokémon",
			"Sword Pokémon", "Royal Sword Pokémon", "Perfume Pokémon", "Fragrance Pokémon", "Cotton Candy Pokémon", "Meringue Pokémon", "Revolving Pokémon", "Overturning Pokémon", "Two-Handed Pokémon", "Collective Pokémon",
			"Mock Kelp Pokémon", "Mock Kelp Pokémon", "Water Gun Pokémon", "Howitzer Pokémon", "Generator Pokémon", "Generator Pokémon", "Royal Heir Pokémon", "Despot Pokémon", "Tundra Pokémon", "Tundra Pokémon",
			"Intertwining Pokémon", "Wrestling Pokémon", "Antenna Pokémon", "Jewel Pokémon", "Soft Tissue Pokémon", "Soft Tissue Pokémon", "Dragon Pokémon", "Key Ring Pokémon", "Stump Pokémon", "Elder Tree Pokémon",
			"Pumpkin Pokémon", "Pumpkin Pokémon", "Ice Chunk Pokémon", "Iceberg Pokémon", "Sound Wave Pokémon", "Sound Wave Pokémon", "Life Pokémon", "Destruction Pokémon", "Order Pokémon", "Jewel Pokémon",
			"Mischief Pokémon", "Steam Pokémon", "Grass Quill Pokémon", "Blade Quill Pokémon", "Arrow Quill Pokémon", "Fire Cat Pokémon", "Fire Cat Pokémon", "Heel Pokémon", "Sea Lion Pokémon", "Pop Star Pokémon",
			"Soloist Pokémon", "Woodpecker Pokémon", "Bugle Beak Pokémon", "Cannon Pokémon", "Loitering Pokémon", "Stakeout Pokémon", "Larva Pokémon", "Battery Pokémon", "Stag Beetle Pokémon", "Boxing Pokémon",
			"Woolly Crab Pokémon", "Dancing Pokémon", "Bee Fly Pokémon", "Bee Fly Pokémon", "Puppy Pokémon", "Wolf Pokémon", "Small Fry Pokémon", "Brutal Star Pokémon", "Brutal Star Pokémon", "Donkey Pokémon",
			"Draft Horse Pokémon", "Water Bubble Pokémon", "Water Bubble Pokémon", "Sickle Grass Pokémon", "Bloom Sickle Pokémon", "Illuminating Pokémon", "Illuminating Pokémon", "Toxic Lizard Pokémon", "Toxic Lizard Pokémon", "Flailing Pokémon",
			"Strong Arm Pokémon", "Fruit Pokémon", "Fruit Pokémon", "Fruit Pokémon", "Posy Picker Pokémon", "Sage Pokémon", "Teamwork Pokémon", "Turn Tail Pokémon", "Hard Scale Pokémon", "Sand Heap Pokémon",
			"Sand Castle Pokémon", "Sea Cucumber Pokémon", "Synthetic Pokémon", "Synthetic Pokémon", "Meteor Pokémon", "Drowsing Pokémon", "Blast Turtle Pokémon", "Roly-Poly Pokémon", "Disguise Pokémon", "Gnash Teeth Pokémon",
			"Placid Pokémon", "Sea Creeper Pokémon", "Scaly Pokémon", "Scaly Pokémon", "Scaly Pokémon", "Land Spirit Pokémon", "Land Spirit Pokémon", "Land Spirit Pokémon", "Land Spirit Pokémon", "Nebula Pokémon",
			"Protostar Pokémon", "Sunne Pokémon", "Moone Pokémon", "Parasite Pokémon", "Swollen Pokémon", "Lissome Pokémon", "Glowing Pokémon", "Launch Pokémon", "Drawn Sword Pokémon", "Junkivore Pokémon",
			"Prism Pokémon", "Artificial Pokémon", "Gloomdweller Pokémon", "Poison Pin Pokémon", "Poison Pin Pokémon", "Rampart Pokémon", "Fireworks Pokémon", "Thunderclap Pokémon", "Hex Nut Pokémon", "Hex Nut Pokémon",
			"Chimp Pokémon", "Beat Pokémon", "Drummer Pokémon", "Rabbit Pokémon", "Rabbit Pokémon", "Striker Pokémon", "Water Lizard Pokémon", "Water Lizard Pokémon", "Secret Agent Pokémon", "Cheeky Pokémon",
			"Greedy Pokémon", "Tiny Bird Pokémon", "Raven Pokémon", "Raven Pokémon", "Larva Pokémon", "Radome Pokémon", "Seven Spot Pokémon", "Fox Pokémon", "Fox Pokémon", "Flowering Pokémon",
			"Cotton Bloom Pokémon", "Sheep Pokémon", "Sheep Pokémon", "Snapping Pokémon", "Bite Pokémon", "Puppy Pokémon", "Dog Pokémon", "Coal Pokémon", "Coal Pokémon", "Coal Pokémon",
			"Apple Core Pokémon", "Apple Wing Pokémon", "Apple Nectar Pokémon", "Sand Snake Pokémon", "Sand Snake Pokémon", "Gulp Pokémon", "Rush Pokémon", "Skewer Pokémon", "Baby Pokémon", "Punk Pokémon",
			"Radiator Pokémon", "Radiator Pokémon", "Tantrum Pokémon", "Jujitsu Pokémon", "Black Tea Pokémon", "Black Tea Pokémon", "Calm Pokémon", "Serene Pokémon", "Silent Pokémon", "Wily Pokémon",
			"Devious Pokémon", "Bulk Up Pokémon", "Blocking Pokémon", "Viking Pokémon", "Coral Pokémon", "Wild Duck Pokémon", "Comedian Pokémon", "Grudge Pokémon", "Cream Pokémon", "Cream Pokémon",
			"Formation Pokémon", "Sea Urchin Pokémon", "Worm Pokémon", "Frost Moth Pokémon", "Big Rock Pokémon", "Penguin Pokémon", "Emotion Pokémon", "Two-Sided Pokémon", "Copperderm Pokémon", "Copperderm Pokémon",
			"Fossil Pokémon", "Fossil Pokémon", "Fossil Pokémon", "Fossil Pokémon", "Alloy Pokémon", "Lingering Pokémon", "Caretaker Pokémon", "Stealth Pokémon", "Warrior Pokémon", "Warrior Pokémon",
			"Gigantic Pokémon", "Wushu Pokémon", "Wushu Pokémon", "Rogue Monkey Pokémon", "Unique Horn Pokémon", "Unique Horn Pokémon", "Dancing Pokémon", "Zen Charm Pokémon", "Djinn Pokémon", "?"
		});
		this.pokedextype.Location = new System.Drawing.Point(403, 80);
		this.pokedextype.Name = "pokedextype";
		this.pokedextype.Size = new System.Drawing.Size(154, 21);
		this.pokedextype.TabIndex = 15;
		this.pokedextype.Visible = false;
		this.Label5.AutoSize = true;
		this.Label5.Location = new System.Drawing.Point(250, 78);
		this.Label5.Name = "Label5";
		this.Label5.Size = new System.Drawing.Size(43, 13);
		this.Label5.TabIndex = 683;
		this.Label5.Text = "Move 4";
		this.Label21.AutoSize = true;
		this.Label21.Enabled = false;
		this.Label21.Location = new System.Drawing.Point(24, 293);
		this.Label21.Name = "Label21";
		this.Label21.Size = new System.Drawing.Size(58, 13);
		this.Label21.TabIndex = 694;
		this.Label21.Text = "Line Count";
		this.Label21.Visible = false;
		this.Label6.AutoSize = true;
		this.Label6.Location = new System.Drawing.Point(250, 26);
		this.Label6.Name = "Label6";
		this.Label6.Size = new System.Drawing.Size(22, 13);
		this.Label6.TabIndex = 684;
		this.Label6.Text = "OT";
		this.pokemonlc.Enabled = false;
		this.pokemonlc.Location = new System.Drawing.Point(90, 290);
		this.pokemonlc.MaxLength = 1;
		this.pokemonlc.Name = "pokemonlc";
		this.pokemonlc.Size = new System.Drawing.Size(154, 20);
		this.pokemonlc.TabIndex = 12;
		this.pokemonlc.Visible = false;
		this.otnamebox.Location = new System.Drawing.Point(310, 21);
		this.otnamebox.MaxLength = 12;
		this.otnamebox.Name = "otnamebox";
		this.otnamebox.Size = new System.Drawing.Size(154, 20);
		this.otnamebox.TabIndex = 6;
		this.otnamebox.TextChanged += new System.EventHandler(otnamebox_TextChanged);
		this.OTG.AllowDrop = true;
		this.OTG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
		this.OTG.FormattingEnabled = true;
		this.OTG.Items.AddRange(new object[2] { "M", "F" });
		this.OTG.Location = new System.Drawing.Point(403, 153);
		this.OTG.Name = "OTG";
		this.OTG.Size = new System.Drawing.Size(36, 21);
		this.OTG.TabIndex = 7;
		this.OTG.Visible = false;
		this.languagebox.AutoCompleteCustomSource.AddRange(new string[7] { "ENG", "FRE", "GER", "ITA", "JPN", "KOR", "SPA" });
		this.languagebox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
		this.languagebox.FormattingEnabled = true;
		this.languagebox.Items.AddRange(new object[12]
		{
			"Unset", "JPN", "ENG", "FRE", "ITA", "GER", "???", "SPA", "KOR", "CHS",
			"CHT", "??2"
		});
		this.languagebox.Location = new System.Drawing.Point(78, 103);
		this.languagebox.Name = "languagebox";
		this.languagebox.Size = new System.Drawing.Size(154, 21);
		this.languagebox.TabIndex = 8;
		this.Label7.AutoSize = true;
		this.Label7.Location = new System.Drawing.Point(12, 106);
		this.Label7.Name = "Label7";
		this.Label7.Size = new System.Drawing.Size(55, 13);
		this.Label7.TabIndex = 688;
		this.Label7.Text = "Language";
		this.tabPage3.Controls.Add(this.itemnameplural);
		this.tabPage3.Controls.Add(this.nitem6);
		this.tabPage3.Controls.Add(this.nitem5);
		this.tabPage3.Controls.Add(this.nitem4);
		this.tabPage3.Controls.Add(this.nitem3);
		this.tabPage3.Controls.Add(this.nitem2);
		this.tabPage3.Controls.Add(this.nitem1);
		this.tabPage3.Controls.Add(this.itemslc);
		this.tabPage3.Controls.Add(this.Label20);
		this.tabPage3.Controls.Add(this.Label18);
		this.tabPage3.Controls.Add(this.Label17);
		this.tabPage3.Controls.Add(this.Label16);
		this.tabPage3.Controls.Add(this.Label15);
		this.tabPage3.Controls.Add(this.Label14);
		this.tabPage3.Controls.Add(this.itembox6);
		this.tabPage3.Controls.Add(this.itembox5);
		this.tabPage3.Controls.Add(this.itembox4);
		this.tabPage3.Controls.Add(this.itembox3);
		this.tabPage3.Controls.Add(this.itembox2);
		this.tabPage3.Controls.Add(this.itembox1);
		this.tabPage3.Controls.Add(this.Label13);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(513, 237);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Items";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.itemnameplural.AllowDrop = true;
		this.itemnameplural.AutoCompleteCustomSource.AddRange(new string[921]
		{
			"None", "Master Ball", "Ultra Ball", "Great Ball", "Poké Ball", "Safari Ball", "Net Ball", "Dive Ball", "Nest Ball", "Repeat Ball",
			"Timer Ball", "Luxury Ball", "Premier Ball", "Dusk Ball", "Heal Ball", "Quick Ball", "Cherish Ball", "Potion", "Antidote", "Burn Heal",
			"Ice Heal", "Awakening", "Paralyze Heal", "Full Restore", "Max Potion", "Hyper Potion", "Super Potion", "Full Heal", "Revive", "Max Revive",
			"Fresh Water", "Soda Pop", "Lemonade", "Moomoo Milk", "Energy Powder", "Energy Root", "Heal Powder", "Revival Herb", "Ether", "Max Ether",
			"Elixir", "Max Elixir", "Lava Cookie", "Berry Juice", "Sacred Ash", "HP Up", "Protein", "Iron", "Carbos", "Calcium",
			"Rare Candy", "PP Up", "Zinc", "PP Max", "Old Gateau", "Guard Spec.", "Dire Hit", "X Attack", "X Defense", "X Speed",
			"X Accuracy", "X Sp. Atk", "X Sp. Def", "Poké Doll", "Fluffy Tail", "Blue Flute", "Yellow Flute", "Red Flute", "Black Flute", "White Flute",
			"Shoal Salt", "Shoal Shell", "Red Shard", "Blue Shard", "Yellow Shard", "Green Shard", "Super Repel", "Max Repel", "Escape Rope", "Repel",
			"Sun Stone", "Moon Stone", "Fire Stone", "Thunder Stone", "Water Stone", "Leaf Stone", "Tiny Mushroom", "Big Mushroom", "Pearl", "Big Pearl",
			"Stardust", "Star Piece", "Nugget", "Heart Scale", "Honey", "Growth Mulch", "Damp Mulch", "Stable Mulch", "Gooey Mulch", "Root Fossil",
			"Claw Fossil", "Helix Fossil", "Dome Fossil", "Old Amber", "Armor Fossil", "Skull Fossil", "Rare Bone", "Shiny Stone", "Dusk Stone", "Dawn Stone",
			"Oval Stone", "Odd Keystone", "Griseous Orb", "???", "???", "???", "Douse Drive", "Shock Drive", "Burn Drive", "Chill Drive",
			"???", "???", "???", "???", "???", "???", "???", "???", "???", "???",
			"???", "???", "???", "???", "Sweet Heart", "Adamant Orb", "Lustrous Orb", "Greet Mail", "Favored Mail", "RSVP Mail",
			"Thanks Mail", "Inquiry Mail", "Like Mail", "Reply Mail", "Bridge Mail S", "Bridge Mail D", "Bridge Mail T", "Bridge Mail V", "Bridge Mail M", "Cheri Berry",
			"Chesto Berry", "Pecha Berry", "Rawst Berry", "Aspear Berry", "Leppa Berry", "Oran Berry", "Persim Berry", "Lum Berry", "Sitrus Berry", "Figy Berry",
			"Wiki Berry", "Mago Berry", "Aguav Berry", "Iapapa Berry", "Razz Berry", "Bluk Berry", "Nanab Berry", "Wepear Berry", "Pinap Berry", "Pomeg Berry",
			"Kelpsy Berry", "Qualot Berry", "Hondew Berry", "Grepa Berry", "Tamato Berry", "Cornn Berry", "Magost Berry", "Rabuta Berry", "Nomel Berry", "Spelon Berry",
			"Pamtre Berry", "Watmel Berry", "Durin Berry", "Belue Berry", "Occa Berry", "Passho Berry", "Wacan Berry", "Rindo Berry", "Yache Berry", "Chople Berry",
			"Kebia Berry", "Shuca Berry", "Coba Berry", "Payapa Berry", "Tanga Berry", "Charti Berry", "Kasib Berry", "Haban Berry", "Colbur Berry", "Babiri Berry",
			"Chilan Berry", "Liechi Berry", "Ganlon Berry", "Salac Berry", "Petaya Berry", "Apicot Berry", "Lansat Berry", "Starf Berry", "Enigma Berry", "Micle Berry",
			"Custap Berry", "Jaboca Berry", "Rowap Berry", "Bright Powder", "White Herb", "Macho Brace", "Exp. Share", "Quick Claw", "Soothe Bell", "Mental Herb",
			"Choice Band", "King’s Rock", "Silver Powder", "Amulet Coin", "Cleanse Tag", "Soul Dew", "Deep Sea Tooth", "Deep Sea Scale", "Smoke Ball", "Everstone",
			"Focus Band", "Lucky Egg", "Scope Lens", "Metal Coat", "Leftovers", "Dragon Scale", "Light Ball", "Soft Sand", "Hard Stone", "Miracle Seed",
			"Black Glasses", "Black Belt", "Magnet", "Mystic Water", "Sharp Beak", "Poison Barb", "Never-Melt Ice", "Spell Tag", "Twisted Spoon", "Charcoal",
			"Dragon Fang", "Silk Scarf", "Up-Grade", "Shell Bell", "Sea Incense", "Lax Incense", "Lucky Punch", "Metal Powder", "Thick Club", "Stick",
			"Red Scarf", "Blue Scarf", "Pink Scarf", "Green Scarf", "Yellow Scarf", "Wide Lens", "Muscle Band", "Wise Glasses", "Expert Belt", "Light Clay",
			"Life Orb", "Power Herb", "Toxic Orb", "Flame Orb", "Quick Powder", "Focus Sash", "Zoom Lens", "Metronome", "Iron Ball", "Lagging Tail",
			"Destiny Knot", "Black Sludge", "Icy Rock", "Smooth Rock", "Heat Rock", "Damp Rock", "Grip Claw", "Choice Scarf", "Sticky Barb", "Power Bracer",
			"Power Belt", "Power Lens", "Power Band", "Power Anklet", "Power Weight", "Shed Shell", "Big Root", "Choice Specs", "Flame Plate", "Splash Plate",
			"Zap Plate", "Meadow Plate", "Icicle Plate", "Fist Plate", "Toxic Plate", "Earth Plate", "Sky Plate", "Mind Plate", "Insect Plate", "Stone Plate",
			"Spooky Plate", "Draco Plate", "Dread Plate", "Iron Plate", "Odd Incense", "Rock Incense", "Full Incense", "Wave Incense", "Rose Incense", "Luck Incense",
			"Pure Incense", "Protector", "Electirizer", "Magmarizer", "Dubious Disc", "Reaper Cloth", "Razor Claw", "Razor Fang", "TM01", "TM02",
			"TM03", "TM04", "TM05", "TM06", "TM07", "TM08", "TM09", "TM10", "TM11", "TM12",
			"TM13", "TM14", "TM15", "TM16", "TM17", "TM18", "TM19", "TM20", "TM21", "TM22",
			"TM23", "TM24", "TM25", "TM26", "TM27", "TM28", "TM29", "TM30", "TM31", "TM32",
			"TM33", "TM34", "TM35", "TM36", "TM37", "TM38", "TM39", "TM40", "TM41", "TM42",
			"TM43", "TM44", "TM45", "TM46", "TM47", "TM48", "TM49", "TM50", "TM51", "TM52",
			"TM53", "TM54", "TM55", "TM56", "TM57", "TM58", "TM59", "TM60", "TM61", "TM62",
			"TM63", "TM64", "TM65", "TM66", "TM67", "TM68", "TM69", "TM70", "TM71", "TM72",
			"TM73", "TM74", "TM75", "TM76", "TM77", "TM78", "TM79", "TM80", "TM81", "TM82",
			"TM83", "TM84", "TM85", "TM86", "TM87", "TM88", "TM89", "TM90", "TM91", "TM92",
			"HM01", "HM02", "HM03", "HM04", "HM05", "HM06", "???", "???", "Explorer Kit", "Loot Sack",
			"Rule Book", "Poké Radar", "Point Card", "Journal", "Seal Case", "Fashion Case", "Seal Bag", "Pal Pad", "Works Key", "Old Charm",
			"Galactic Key", "Red Chain", "Town Map", "Vs. Seeker", "Coin Case", "Old Rod", "Good Rod", "Super Rod", "Sprayduck", "Poffin Case",
			"Bike", "Suite Key", "Oak’s Letter", "Lunar Wing", "Member Card", "Azure Flute", "S.S. Ticket", "Contest Pass", "Magma Stone", "Parcel",
			"Coupon 1", "Coupon 2", "Coupon 3", "Storage Key", "Secret Potion", "Vs. Recorder", "Gracidea", "Secret Key", "Apricorn Box", "Unown Report",
			"Berry Pots", "Dowsing Machine", "Blue Card", "Slowpoke Tail", "Clear Bell", "Card Key", "Basement Key", "Squirt Bottle", "Red Scale", "Lost Item",
			"Pass", "Machine Part", "Silver Wing", "Rainbow Wing", "Mystery Egg", "Red Apricorn", "Blue Apricorn", "Yellow Apricorn", "Green Apricorn", "Pink Apricorn",
			"White Apricorn", "Black Apricorn", "Fast Ball", "Level Ball", "Lure Ball", "Heavy Ball", "Love Ball", "Friend Ball", "Moon Ball", "Sport Ball",
			"Park Ball", "Photo Album", "GB Sounds", "Tidal Bell", "Rage Candy Bar", "Data Card 01", "Data Card 02", "Data Card 03", "Data Card 04", "Data Card 05",
			"Data Card 06", "Data Card 07", "Data Card 08", "Data Card 09", "Data Card 10", "Data Card 11", "Data Card 12", "Data Card 13", "Data Card 14", "Data Card 15",
			"Data Card 16", "Data Card 17", "Data Card 18", "Data Card 19", "Data Card 20", "Data Card 21", "Data Card 22", "Data Card 23", "Data Card 24", "Data Card 25",
			"Data Card 26", "Data Card 27", "Jade Orb", "Lock Capsule", "Red Orb", "Blue Orb", "Enigma Stone", "Prism Scale", "Eviolite", "Float Stone",
			"Rocky Helmet", "Air Balloon", "Red Card", "Ring Target", "Binding Band", "Absorb Bulb", "Cell Battery", "Eject Button", "Fire Gem", "Water Gem",
			"Electric Gem", "Grass Gem", "Ice Gem", "Fighting Gem", "Poison Gem", "Ground Gem", "Flying Gem", "Psychic Gem", "Bug Gem", "Rock Gem",
			"Ghost Gem", "Dragon Gem", "Dark Gem", "Steel Gem", "Normal Gem", "Health Wing", "Muscle Wing", "Resist Wing", "Genius Wing", "Clever Wing",
			"Swift Wing", "Pretty Wing", "Cover Fossil", "Plume Fossil", "Liberty Pass", "Pass Orb", "Dream Ball", "Poké Toy", "Prop Case", "Dragon Skull",
			"Balm Mushroom", "Big Nugget", "Pearl String", "Comet Shard", "Relic Copper", "Relic Silver", "Relic Gold", "Relic Vase", "Relic Band", "Relic Statue",
			"Relic Crown", "Casteliacone", "Dire Hit 2", "X Speed 2", "X Sp. Atk 2", "X Sp. Def 2", "X Defense 2", "X Attack 2", "X Accuracy 2", "X Speed 3",
			"X Sp. Atk 3", "X Sp. Def 3", "X Defense 3", "X Attack 3", "X Accuracy 3", "X Speed 6", "X Sp. Atk 6", "X Sp. Def 6", "X Defense 6", "X Attack 6",
			"X Accuracy 6", "Ability Urge", "Item Drop", "Item Urge", "Reset Urge", "Dire Hit 3", "Light Stone", "Dark Stone", "TM93", "TM94",
			"TM95", "Xtransceiver", "???", "Gram 1", "Gram 2", "Gram 3", "Xtransceiver", "Medal Box", "DNA Splicers", "DNA Splicers",
			"Permit", "Oval Charm", "Shiny Charm", "Plasma Card", "Grubby Hanky", "Colress Machine", "Dropped Item", "Dropped Item", "Reveal Glass", "Weakness Policy",
			"Assault Vest", "Holo Caster", "Prof’s Letter", "Roller Skates", "Pixie Plate", "Ability Capsule", "Whipped Dream", "Sachet", "Luminous Moss", "Snowball",
			"Safety Goggles", "Poké Flute", "Rich Mulch", "Surprise Mulch", "Boost Mulch", "Amaze Mulch", "Gengarite", "Gardevoirite", "Ampharosite", "Venusaurite",
			"Charizardite X", "Blastoisinite", "Mewtwonite X", "Mewtwonite Y", "Blazikenite", "Medichamite", "Houndoominite", "Aggronite", "Banettite", "Tyranitarite",
			"Scizorite", "Pinsirite", "Aerodactylite", "Lucarionite", "Abomasite", "Kangaskhanite", "Gyaradosite", "Absolite", "Charizardite Y", "Alakazite",
			"Heracronite", "Mawilite", "Manectite", "Garchompite", "Latiasite", "Latiosite", "Roseli Berry", "Kee Berry", "Maranga Berry", "Sprinklotad",
			"TM96", "TM97", "TM98", "TM99", "TM100", "Power Plant Pass", "Mega Ring", "Intriguing Stone", "Common Stone", "Discount Coupon",
			"Elevator Key", "TMV Pass", "Honor of Kalos", "Adventure Rules", "Strange Souvenir", "Lens Case", "Makeup Bag", "Travel Trunk", "Lumiose Galette", "Shalour Sable",
			"Jaw Fossil", "Sail Fossil", "Looker Ticket", "Bike", "Holo Caster", "Fairy Gem", "Mega Charm", "Mega Glove", "Mach Bike", "Acro Bike",
			"Wailmer Pail", "Devon Parts", "Soot Sack", "Basement Key", "Pokéblock Kit", "Letter", "Eon Ticket", "Scanner", "Go-Goggles", "Meteorite",
			"Key to Room 1", "Key to Room 2", "Key to Room 4", "Key to Room 6", "Storage Key", "Devon Scope", "S.S. Ticket", "HM07", "Devon Scuba Gear", "Contest Costume",
			"Contest Costume", "Magma Suit", "Aqua Suit", "Pair of Tickets", "Mega Bracelet", "Mega Pendant", "Mega Glasses", "Mega Anchor", "Mega Stickpin", "Mega Tiara",
			"Mega Anklet", "Meteorite", "Swampertite", "Sceptilite", "Sablenite", "Altarianite", "Galladite", "Audinite", "Metagrossite", "Sharpedonite",
			"Slowbronite", "Steelixite", "Pidgeotite", "Glalitite", "Diancite", "Prison Bottle", "Mega Cuff", "Cameruptite", "Lopunnite", "Salamencite",
			"Beedrillite", "Meteorite", "Meteorite", "Key Stone", "Meteorite Shard", "Eon Flute", "n/a", "n/a", "n/a", "Electrium Z",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "Z-Ring", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"Electrium Z", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "UB Ball *", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a"
		});
		this.itemnameplural.FormattingEnabled = true;
		this.itemnameplural.Items.AddRange(new object[1608]
		{
			"None", "Master Balls", "Ultra Balls", "Great Balls", "Poké Balls", "Safari Balls", "Net Balls", "Dive Balls", "Nest Balls", "Repeat Balls",
			"Timer Balls", "Luxury Balls", "Premier Balls", "Dusk Balls", "Heal Balls", "Quick Balls", "Cherish Balls", "Potions", "Antidotes", "Burn Heals",
			"Ice Heals", "Awakenings", "Paralyze Heals", "Full Restores", "Max Potions", "Hyper Potions", "Super Potions", "Full Heals", "Revives", "Max Revives",
			"Fresh Waters", "Soda Pops", "Lemonades", "Moomoo Milks", "Energy Powders", "Energy Roots", "Heal Powders", "Revival Herbs", "Ethers", "Max Ethers",
			"Elixirs", "Max Elixirs", "Lava Cookies", "Berry Juices", "Sacred Ashes", "HP Ups", "Proteins", "Irons", "Carbos", "Calciums",
			"Rare Candies", "PP Ups", "Zincs", "PP Maxes", "Old Gateaux", "Guard Specs.", "Dire Hits", "X Attacks", "X Defenses", "X Speeds",
			"X Accuracies", "X Sp. Atks", "X Sp. Defs", "Poké Dolls", "Fluffy Tails", "Blue Flutes", "Yellow Flutes", "Red Flutes", "Black Flutes", "White Flutes",
			"Shoal Salts", "Shoal Shells", "Red Shards", "Blue Shards", "Yellow Shards", "Green Shards", "Super Repels", "Max Repels", "Escape Ropes", "Repels",
			"Sun Stones", "Moon Stones", "Fire Stones", "Thunder Stones", "Water Stones", "Leaf Stones", "Tiny Mushrooms", "Big Mushrooms", "Pearls", "Big Pearls",
			"Stardusts", "Star Pieces", "Nuggets", "Heart Scales", "Honey", "Growth Mulch", "Damp Mulch", "Stable Mulch", "Gooey Mulch", "Root Fossils",
			"Claw Fossils", "Helix Fossils", "Dome Fossils", "Old Ambers", "Armor Fossils", "Skull Fossils", "Rare Bones", "Shiny Stones", "Dusk Stones", "Dawn Stones",
			"Oval Stones", "Odd Keystones", "Griseous Orbs", "Tea", "???", "Autographs", "Douse Drives", "Shock Drives", "Burn Drives", "Chill Drives",
			"???", "Pokémon Box Links", "Medicine Pockets", "TM Cases", "Candy Jars", "Power-Up Pockets", "Clothing Trunks", "Catching Pockets", "Battle Pockets", "???",
			"???", "???", "???", "???", "Sweet Hearts", "Adamant Orbs", "Lustrous Orbs", "Greet Mail", "Favored Mail", "RSVP Mail",
			"Thanks Mail", "Inquiry Mail", "Like Mail", "Reply Mail", "Bridge Mail S", "Bridge Mail D", "Bridge Mail T", "Bridge Mail V", "Bridge Mail M", "Cheri Berries",
			"Chesto Berries", "Pecha Berries", "Rawst Berries", "Aspear Berries", "Leppa Berries", "Oran Berries", "Persim Berries", "Lum Berries", "Sitrus Berries", "Figy Berries",
			"Wiki Berries", "Mago Berries", "Aguav Berries", "Iapapa Berries", "Razz Berries", "Bluk Berries", "Nanab Berries", "Wepear Berries", "Pinap Berries", "Pomeg Berries",
			"Kelpsy Berries", "Qualot Berries", "Hondew Berries", "Grepa Berries", "Tamato Berries", "Cornn Berries", "Magost Berries", "Rabuta Berries", "Nomel Berries", "Spelon Berries",
			"Pamtre Berries", "Watmel Berries", "Durin Berries", "Belue Berries", "Occa Berries", "Passho Berries", "Wacan Berries", "Rindo Berries", "Yache Berries", "Chople Berries",
			"Kebia Berries", "Shuca Berries", "Coba Berries", "Payapa Berries", "Tanga Berries", "Charti Berries", "Kasib Berries", "Haban Berries", "Colbur Berries", "Babiri Berries",
			"Chilan Berries", "Liechi Berries", "Ganlon Berries", "Salac Berries", "Petaya Berries", "Apicot Berries", "Lansat Berries", "Starf Berries", "Enigma Berries", "Micle Berries",
			"Custap Berries", "Jaboca Berries", "Rowap Berries", "Bright Powders", "White Herbs", "Macho Braces", "Exp. Shares", "Quick Claws", "Soothe Bells", "Mental Herbs",
			"Choice Bands", "King’s Rocks", "Silver Powders", "Amulet Coins", "Cleanse Tags", "Soul Dews", "Deep Sea Teeth", "Deep Sea Scales", "Smoke Balls", "Everstones",
			"Focus Bands", "Lucky Eggs", "Scope Lenses", "Metal Coats", "Leftovers", "Dragon Scales", "Light Balls", "Soft Sand", "Hard Stones", "Miracle Seeds",
			"Black Glasses", "Black Belts", "Magnets", "Mystic Waters", "Sharp Beaks", "Poison Barbs", "Never-Melt Ices", "Spell Tags", "Twisted Spoons", "Charcoals",
			"Dragon Fangs", "Silk Scarves", "Upgrades", "Shell Bells", "Sea Incenses", "Lax Incenses", "Lucky Punches", "Metal Powders", "Thick Clubs", "Leeks",
			"Red Scarves", "Blue Scarves", "Pink Scarves", "Green Scarves", "Yellow Scarves", "Wide Lenses", "Muscle Bands", "Wise Glasses", "Expert Belts", "Light Clays",
			"Life Orbs", "Power Herbs", "Toxic Orbs", "Flame Orbs", "Quick Powders", "Focus Sashes", "Zoom Lenses", "Metronomes", "Iron Balls", "Lagging Tails",
			"Destiny Knots", "Black Sludges", "Icy Rocks", "Smooth Rocks", "Heat Rocks", "Damp Rocks", "Grip Claws", "Choice Scarves", "Sticky Barbs", "Power Bracers",
			"Power Belts", "Power Lenses", "Power Bands", "Power Anklets", "Power Weights", "Shed Shells", "Big Roots", "Choice Specs", "Flame Plates", "Splash Plates",
			"Zap Plates", "Meadow Plates", "Icicle Plates", "Fist Plates", "Toxic Plates", "Earth Plates", "Sky Plates", "Mind Plates", "Insect Plates", "Stone Plates",
			"Spooky Plates", "Draco Plates", "Dread Plates", "Iron Plates", "Odd Incenses", "Rock Incenses", "Full Incenses", "Wave Incenses", "Rose Incenses", "Luck Incenses",
			"Pure Incenses", "Protectors", "Electirizers", "Magmarizers", "Dubious Discs", "Reaper Cloths", "Razor Claws", "Razor Fangs", "TM01s", "TM02s",
			"TM03s", "TM04s", "TM05s", "TM06s", "TM07s", "TM08s", "TM09s", "TM10s", "TM11s", "TM12s",
			"TM13s", "TM14s", "TM15s", "TM16s", "TM17s", "TM18s", "TM19s", "TM20s", "TM21s", "TM22s",
			"TM23s", "TM24s", "TM25s", "TM26s", "TM27s", "TM28s", "TM29s", "TM30s", "TM31s", "TM32s",
			"TM33s", "TM34s", "TM35s", "TM36s", "TM37s", "TM38s", "TM39s", "TM40s", "TM41s", "TM42s",
			"TM43s", "TM44s", "TM45s", "TM46s", "TM47s", "TM48s", "TM49s", "TM50s", "TM51s", "TM52s",
			"TM53s", "TM54s", "TM55s", "TM56s", "TM57s", "TM58s", "TM59s", "TM60s", "TM61s", "TM62s",
			"TM63s", "TM64s", "TM65s", "TM66s", "TM67s", "TM68s", "TM69s", "TM70s", "TM71s", "TM72s",
			"TM73s", "TM74s", "TM75s", "TM76s", "TM77s", "TM78s", "TM79s", "TM80s", "TM81s", "TM82s",
			"TM83s", "TM84s", "TM85s", "TM86s", "TM87s", "TM88s", "TM89s", "TM90s", "TM91s", "TM92s",
			"HM01s", "HM02s", "HM03s", "HM04s", "HM05s", "HM06s", "???", "???", "Explorer Kits", "Loot Sacks",
			"Rule Books", "Poké Radars", "Point Cards", "Journals", "Seal Cases", "Fashion Cases", "Seal Bags", "Pal Pads", "Works Keys", "Old Charms",
			"Galactic Keys", "Red Chains", "Town Maps", "Vs. Seekers", "Coin Cases", "Old Rods", "Good Rods", "Super Rods", "Sprayducks", "Poffin Cases",
			"Bikes", "Suite Keys", "Oak’s Letters", "Lunar Wings", "Member Cards", "Azure Flutes", "S.S. Tickets", "Contest Passes", "Magma Stones", "Parcels",
			"Coupon 1s", "Coupon 2s", "Coupon 3s", "Storage Keys", "Secret Potions", "Vs. Recorders", "Gracideas", "Secret Keys", "Apricorn Boxes", "Unown Reports",
			"Berry Pots", "Dowsing Machines", "Blue Cards", "Slowpoke Tails", "Clear Bells", "Card Keys", "Basement Keys", "Squirt Bottles", "Red Scales", "Lost Items",
			"Passes", "Machine Parts", "Silver Wings", "Rainbow Wings", "Mystery Eggs", "Red Apricorns", "Blue Apricorns", "Yellow Apricorns", "Green Apricorns", "Pink Apricorns",
			"White Apricorns", "Black Apricorns", "Fast Balls", "Level Balls", "Lure Balls", "Heavy Balls", "Love Balls", "Friend Balls", "Moon Balls", "Sport Balls",
			"Park Balls", "Photo Albums", "GB Sounds", "Tidal Bells", "Rage Candy Bars", "Data Card 01s", "Data Card 02s", "Data Card 03s", "Data Card 04s", "Data Card 05s",
			"Data Card 06s", "Data Card 07s", "Data Card 08s", "Data Card 09s", "Data Card 10s", "Data Card 11s", "Data Card 12s", "Data Card 13s", "Data Card 14s", "Data Card 15s",
			"Data Card 16s", "Data Card 17s", "Data Card 18s", "Data Card 19s", "Data Card 20s", "Data Card 21s", "Data Card 22s", "Data Card 23s", "Data Card 24s", "Data Card 25s",
			"Data Card 26s", "Data Card 27s", "Jade Orbs", "Lock Capsules", "Red Orbs", "Blue Orbs", "Enigma Stones", "Prism Scales", "Eviolites", "Float Stones",
			"Rocky Helmets", "Air Balloons", "Red Cards", "Ring Targets", "Binding Bands", "Absorb Bulbs", "Cell Batteries", "Eject Buttons", "Fire Gems", "Water Gems",
			"Electric Gems", "Grass Gems", "Ice Gems", "Fighting Gems", "Poison Gems", "Ground Gems", "Flying Gems", "Psychic Gems", "Bug Gems", "Rock Gems",
			"Ghost Gems", "Dragon Gems", "Dark Gems", "Steel Gems", "Normal Gems", "Health Feathers", "Muscle Feathers", "Resist Feathers", "Genius Feathers", "Clever Feathers",
			"Swift Feathers", "Pretty Feathers", "Cover Fossils", "Plume Fossils", "Liberty Passes", "Pass Orbs", "Dream Balls", "Poké Toys", "Prop Cases", "Dragon Skulls",
			"Balm Mushrooms", "Big Nuggets", "Pearl Strings", "Comet Shards", "Relic Coppers", "Relic Silvers", "Relic Golds", "Relic Vases", "Relic Bands", "Relic Statues",
			"Relic Crowns", "Casteliacones", "Dire Hit 2s", "X Speed 2s", "X Sp. Atk 2s", "X Sp. Def 2s", "X Defense 2s", "X Attack 2s", "X Accuracy 2s", "X Speed 3s",
			"X Sp. Atk 3s", "X Sp. Def 3s", "X Defense 3s", "X Attack 3s", "X Accuracy 3s", "X Speed 6s", "X Sp. Atk 6s", "X Sp. Def 6s", "X Defense 6s", "X Attack 6s",
			"X Accuracy 6s", "Ability Urges", "Item Drops", "Item Urges", "Reset Urges", "Dire Hit 3s", "Light Stones", "Dark Stones", "TM93s", "TM94s",
			"TM95s", "Xtransceivers", "???", "Gram 1s", "Gram 2s", "Gram 3s", "Xtransceivers", "Medal Boxes", "DNA Splicers", "DNA Splicers",
			"Permits", "Oval Charms", "Shiny Charms", "Plasma Cards", "Grubby Hankies", "Colress Machines", "Dropped Items", "Dropped Items", "Reveal Glasses", "Weakness Policies",
			"Assault Vests", "Holo Casters", "Prof’s Letters", "Roller Skates", "Pixie Plates", "Ability Capsules", "Whipped Dreams", "Sachets", "Luminous Moss", "Snowballs",
			"Safety Goggles", "Poké Flutes", "Rich Mulch", "Surprise Mulch", "Boost Mulch", "Amaze Mulch", "Gengarites", "Gardevoirites", "Ampharosites", "Venusaurites",
			"Charizardite Xs", "Blastoisinites", "Mewtwonite Xs", "Mewtwonite Ys", "Blazikenites", "Medichamites", "Houndoominites", "Aggronites", "Banettites", "Tyranitarites",
			"Scizorites", "Pinsirites", "Aerodactylites", "Lucarionites", "Abomasites", "Kangaskhanites", "Gyaradosites", "Absolites", "Charizardite Ys", "Alakazites",
			"Heracronites", "Mawilites", "Manectites", "Garchompites", "Latiasites", "Latiosites", "Roseli Berries", "Kee Berries", "Maranga Berries", "Sprinklotads",
			"TM96s", "TM97s", "TM98s", "TM99s", "TM100s", "Power Plant Passes", "Mega Rings", "Intriguing Stones", "Common Stones", "Discount Coupons",
			"Elevator Keys", "TMV Passes", "Honors of Kalos", "Adventure Guides", "Strange Souvenirs", "Lens Cases", "Makeup Bags", "Travel Trunks", "Lumiose Galettes", "Shalour Sables",
			"Jaw Fossils", "Sail Fossils", "Looker Tickets", "Bikes", "Holo Casters", "Fairy Gems", "Mega Charms", "Mega Gloves", "Mach Bikes", "Acro Bikes",
			"Wailmer Pails", "Devon Parts", "Soot Sacks", "Basement Keys", "Pokéblock Kits", "Letters", "Eon Tickets", "Scanners", "Go-Goggles", "Meteorites",
			"Keys to Room 1", "Keys to Room 2", "Keys to Room 4", "Keys to Room 6", "Storage Keys", "Devon Scopes", "S.S. Tickets", "HM07s", "Devon Scuba Gear", "Contest Costumes",
			"Contest Costumes", "Magma Suits", "Aqua Suits", "Pair of Tickets", "Mega Bracelets", "Mega Pendants", "Mega Glasses", "Mega Anchors", "Mega Stickpins", "Mega Tiaras",
			"Mega Anklets", "Meteorites", "Swampertites", "Sceptilites", "Sablenites", "Altarianites", "Galladites", "Audinites", "Metagrossites", "Sharpedonites",
			"Slowbronites", "Steelixites", "Pidgeotites", "Glalitites", "Diancites", "Prison Bottles", "Mega Cuffs", "Cameruptites", "Lopunnites", "Salamencites",
			"Beedrillites", "Meteorites", "Meteorites", "Key Stones", "Meteorite Shards", "Eon Flutes", "Normalium Zs", "Firium Zs", "Waterium Zs", "Electrium Zs",
			"Grassium Zs", "Icium Zs", "Fightinium Zs", "Poisonium Zs", "Groundium Zs", "Flyinium Zs", "Psychium Zs", "Buginium Zs", "Rockium Zs", "Ghostium Zs",
			"Dragonium Zs", "Darkinium Zs", "Steelium Zs", "Fairium Zs", "Pikanium Zs", "Bottle Caps", "Gold Bottle Caps", "Z-Rings", "Decidium Zs", "Incinium Zs",
			"Primarium Zs", "Tapunium Zs", "Marshadium Zs", "Aloraichium Zs", "Snorlium Zs", "Eevium Zs", "Mewnium Zs", "Normalium Zs", "Firium Zs", "Waterium Zs",
			"Electrium Zs", "Grassium Zs", "Icium Zs", "Fightinium Zs", "Poisonium Zs", "Groundium Zs", "Flyinium Zs", "Psychium Zs", "Buginium Zs", "Rockium Zs",
			"Ghostium Zs", "Dragonium Zs", "Darkinium Zs", "Steelium Zs", "Fairium Zs", "Pikanium Zs", "Decidium Zs", "Incinium Zs", "Primarium Zs", "Tapunium Zs",
			"Marshadium Zs", "Aloraichium Zs", "Snorlium Zs", "Eevium Zs", "Mewnium Zs", "Pikashunium Zs", "Pikashunium Zs", "???", "???", "???",
			"???", "Forage Bags", "Fishing Rods", "Professor’s Masks", "Festival Tickets", "Sparkling Stones", "Adrenaline Orbs", "Zygarde Cubes", "???", "Ice Stones",
			"Ride Pagers", "Beast Balls", "Big Malasadas", "Red Nectars", "Yellow Nectars", "Pink Nectars", "Purple Nectars", "Sun Flutes", "Moon Flutes", "???",
			"Enigmatic Cards", "Silver Razz Berries", "Golden Razz Berries", "Silver Nanab Berries", "Golden Nanab Berries", "Silver Pinap Berries", "Golden Pinap Berries", "???", "???", "???",
			"???", "???", "Secret Keys", "S.S. Tickets", "Silph Scopes", "Parcels", "Card Keys", "Gold Teeth", "Lift Keys", "Terrain Extenders",
			"Protective Pads", "Electric Seeds", "Psychic Seeds", "Misty Seeds", "Grassy Seeds", "Stretchy Springs", "Chalky Stones", "Marbles", "Lone Earrings", "Beach Glass",
			"Gold Leaves", "Silver Leaves", "Polished Mud Balls", "Tropical Shells", "Leaf Letters", "Leaf Letters", "Small Bouquets", "???", "???", "???",
			"Lures", "Super Lures", "Max Lures", "Pewter Crunchies", "Fighting Memories", "Flying Memories", "Poison Memories", "Ground Memories", "Rock Memories", "Bug Memories",
			"Ghost Memories", "Steel Memories", "Fire Memories", "Water Memories", "Grass Memories", "Electric Memories", "Psychic Memories", "Ice Memories", "Dragon Memories", "Dark Memories",
			"Fairy Memories", "Solganium Zs", "Lunalium Zs", "Ultranecrozium Zs", "Mimikium Zs", "Lycanium Zs", "Kommonium Zs", "Solganium Zs", "Lunalium Zs", "Ultranecrozium Zs",
			"Mimikium Zs", "Lycanium Zs", "Kommonium Zs", "Z-Power Rings", "Pink Petals", "Orange Petals", "Blue Petals", "Red Petals", "Green Petals", "Yellow Petals",
			"Purple Petals", "Rainbow Flowers", "Surge Badges", "N-Solarizers", "N-Lunarizers", "N-Solarizers", "N-Lunarizers", "Ilima Normalium Zs", "Left Poké Balls", "Roto Hatches",
			"Roto Bargains", "Roto Prize Money", "Roto Exp. Points", "Roto Friendships", "Roto Encounters", "Roto Stealths", "Roto HP Restores", "Roto PP Restores", "Roto Boosts", "Roto Catches",
			"Health Candies", "Mighty Candies", "Tough Candies", "Smart Candies", "Courage Candies", "Quick Candies", "Health Candies L", "Mighty Candies L", "Tough Candies L", "Smart Candies L",
			"Courage Candies L", "Quick Candies L", "Health Candies XL", "Mighty Candies XL", "Tough Candies XL", "Smart Candies XL", "Courage Candies XL", "Quick Candies XL", "Bulbasaur Candies", "Charmander Candies",
			"Squirtle Candies", "Caterpie Candies", "Weedle Candies", "Pidgey Candies", "Rattata Candies", "Spearow Candies", "Ekans Candies", "Pikachu Candies", "Sandshrew Candies", "Nidoran♀ Candies",
			"Nidoran♂ Candies", "Clefairy Candies", "Vulpix Candies", "Jigglypuff Candies", "Zubat Candies", "Oddish Candies", "Paras Candies", "Venonat Candies", "Diglett Candies", "Meowth Candies",
			"Psyduck Candies", "Mankey Candies", "Growlithe Candies", "Poliwag Candies", "Abra Candies", "Machop Candies", "Bellsprout Candies", "Tentacool Candies", "Geodude Candies", "Ponyta Candies",
			"Slowpoke Candies", "Magnemite Candies", "Farfetch’d Candies", "Doduo Candies", "Seel Candies", "Grimer Candies", "Shellder Candies", "Gastly Candies", "Onix Candies", "Drowzee Candies",
			"Krabby Candies", "Voltorb Candies", "Exeggcute Candies", "Cubone Candies", "Hitmonlee Candies", "Hitmonchan Candies", "Lickitung Candies", "Koffing Candies", "Rhyhorn Candies", "Chansey Candies",
			"Tangela Candies", "Kangaskhan Candies", "Horsea Candies", "Goldeen Candies", "Staryu Candies", "Mr. Mime Candies", "Scyther Candies", "Jynx Candies", "Electabuzz Candies", "Pinsir Candies",
			"Tauros Candies", "Magikarp Candies", "Lapras Candies", "Ditto Candies", "Eevee Candies", "Porygon Candies", "Omanyte Candies", "Kabuto Candies", "Aerodactyl Candies", "Snorlax Candies",
			"Articuno Candies", "Zapdos Candies", "Moltres Candies", "Dratini Candies", "Mewtwo Candies", "Mew Candies", "Meltan Candies", "Magmar Candies", "???", "???",
			"???", "???", "???", "???", "???", "???", "???", "???", "???", "???",
			"???", "???", "???", "???", "Endorsements", "Pokémon Box Links", "Wishing Stars", "Dynamax Bands", "???", "???",
			"Fishing Rods", "Rotom Bikes", "???", "???", "Sausages", "Bob’s Food Tins", "Bach’s Food Tins", "Tins of Beans", "Bread", "Pasta",
			"Mixed Mushrooms", "Smoke-Poke Tails", "Large Leeks", "Fancy Apples", "Brittle Bones", "Packs of Potatoes", "Pungent Roots", "Salad Mixes", "Fried Foods", "Boiled Eggs",
			"Camping Gears", "???", "???", "Rusted Swords", "Rusted Shields", "Fossilized Birds", "Fossilized Fish", "Fossilized Drakes", "Fossilized Dinos", "Strawberry Sweets",
			"Love Sweets", "Berry Sweets", "Clover Sweets", "Flower Sweets", "Star Sweets", "Ribbon Sweets", "Sweet Apples", "Tart Apples", "Throat Sprays", "Eject Packs",
			"Heavy-Duty Boots", "Blunder Policies", "Room Service", "Utility Umbrellas", "Exp. Candies XS", "Exp. Candies S", "Exp. Candies M", "Exp. Candies L", "Exp. Candies XL", "Dynamax Candies",
			"TR00s", "TR01s", "TR02s", "TR03s", "TR04s", "TR05s", "TR06s", "TR07s", "TR08s", "TR09s",
			"TR10s", "TR11s", "TR12s", "TR13s", "TR14s", "TR15s", "TR16s", "TR17s", "TR18s", "TR19s",
			"TR20s", "TR21s", "TR22s", "TR23s", "TR24s", "TR25s", "TR26s", "TR27s", "TR28s", "TR29s",
			"TR30s", "TR31s", "TR32s", "TR33s", "TR34s", "TR35s", "TR36s", "TR37s", "TR38s", "TR39s",
			"TR40s", "TR41s", "TR42s", "TR43s", "TR44s", "TR45s", "TR46s", "TR47s", "TR48s", "TR49s",
			"TR50s", "TR51s", "TR52s", "TR53s", "TR54s", "TR55s", "TR56s", "TR57s", "TR58s", "TR59s",
			"TR60s", "TR61s", "TR62s", "TR63s", "TR64s", "TR65s", "TR66s", "TR67s", "TR68s", "TR69s",
			"TR70s", "TR71s", "TR72s", "TR73s", "TR74s", "TR75s", "TR76s", "TR77s", "TR78s", "TR79s",
			"TR80s", "TR81s", "TR82s", "TR83s", "TR84s", "TR85s", "TR86s", "TR87s", "TR88s", "TR89s",
			"TR90s", "TR91s", "TR92s", "TR93s", "TR94s", "TR95s", "TR96s", "TR97s", "TR98s", "TR99s",
			"TM00s", "Lonely Mints", "Adamant Mints", "Naughty Mints", "Brave Mints", "Bold Mints", "Impish Mints", "Lax Mints", "Relaxed Mints", "Modest Mints",
			"Mild Mints", "Rash Mints", "Quiet Mints", "Calm Mints", "Gentle Mints", "Careful Mints", "Sassy Mints", "Timid Mints", "Hasty Mints", "Jolly Mints",
			"Naive Mints", "Serious Mints", "Wishing Pieces", "Cracked Pots", "Chipped Pots", "Hi-tech Earbuds", "Fruit Bunches", "Moomoo Cheese", "Spice Mix", "Fresh Cream",
			"Packaged Curry", "Coconut Milk", "Instant Noodles", "Precooked Burgers", "Gigantamixes", "Wishing Chips", "Rotom Bikes", "Catching Charms", "???", "Old Letters",
			"Band Autographs", "Sonia’s Books", "???", "???", "???", "???", "???", "???", "Rotom Catalogs", "★And458s",
			"★And15s", "★And337s", "★And603s", "★And390s", "★Sgr6879s", "★Sgr6859s", "★Sgr6913s", "★Sgr7348s", "★Sgr7121s", "★Sgr6746s",
			"★Sgr7194s", "★Sgr7337s", "★Sgr7343s", "★Sgr6812s", "★Sgr7116s", "★Sgr7264s", "★Sgr7597s", "★Del7882s", "★Del7906s", "★Del7852s",
			"★Psc596s", "★Psc361s", "★Psc510s", "★Psc437s", "★Psc8773s", "★Lep1865s", "★Lep1829s", "★Boo5340s", "★Boo5506s", "★Boo5435s",
			"★Boo5602s", "★Boo5733s", "★Boo5235s", "★Boo5351s", "★Hya3748s", "★Hya3903s", "★Hya3418s", "★Hya3482s", "★Hya3845s", "★Eri1084s",
			"★Eri472s", "★Eri1666s", "★Eri897s", "★Eri1231s", "★Eri874s", "★Eri1298s", "★Eri1325s", "★Eri984s", "★Eri1464s", "★Eri1393s",
			"★Eri850s", "★Tau1409s", "★Tau1457s", "★Tau1165s", "★Tau1791s", "★Tau1910s", "★Tau1346s", "★Tau1373s", "★Tau1412s", "★CMa2491s",
			"★CMa2693s", "★CMa2294s", "★CMa2827s", "★CMa2282s", "★CMa2618s", "★CMa2657s", "★CMa2646s", "★UMa4905s", "★UMa4301s", "★UMa5191s",
			"★UMa5054s", "★UMa4295s", "★UMa4660s", "★UMa4554s", "★UMa4069s", "★UMa3569s", "★UMa3323s", "★UMa4033s", "★UMa4377s", "★UMa4375s",
			"★UMa4518s", "★UMa3594s", "★Vir5056s", "★Vir4825s", "★Vir4932s", "★Vir4540s", "★Vir4689s", "★Vir5338s", "★Vir4910s", "★Vir5315s",
			"★Vir5359s", "★Vir5409s", "★Vir5107s", "★Ari617s", "★Ari553s", "★Ari546s", "★Ari951s", "★Ori1713s", "★Ori2061s", "★Ori1790s",
			"★Ori1903s", "★Ori1948s", "★Ori2004s", "★Ori1852s", "★Ori1879s", "★Ori1899s", "★Ori1543s", "★Cas21s", "★Cas168s", "★Cas403s",
			"★Cas153s", "★Cas542s", "★Cas219s", "★Cas265s", "★Cnc3572s", "★Cnc3208s", "★Cnc3461s", "★Cnc3449s", "★Cnc3429s", "★Cnc3627s",
			"★Cnc3268s", "★Cnc3249s", "★Com4968s", "★Crv4757s", "★Crv4623s", "★Crv4662s", "★Crv4786s", "★Aur1708s", "★Aur2088s", "★Aur1605s",
			"★Aur2095s", "★Aur1577s", "★Aur1641s", "★Aur1612s", "★Pav7790s", "★Cet911s", "★Cet681s", "★Cet188s", "★Cet539s", "★Cet804s",
			"★Cep8974s", "★Cep8162s", "★Cep8238s", "★Cep8417s", "★Cen5267s", "★Cen5288s", "★Cen551s", "★Cen5459s", "★Cen5460s", "★CMi2943s",
			"★CMi2845s", "★Equ8131s", "★Vul7405s", "★UMi424s", "★UMi5563s", "★UMi5735s", "★UMi6789s", "★Crt4287s", "★Lyr7001s", "★Lyr7178s",
			"★Lyr7106s", "★Lyr7298s", "★Ara6585s", "★Sco6134s", "★Sco6527s", "★Sco6553s", "★Sco5953s", "★Sco5984s", "★Sco6508s", "★Sco6084s",
			"★Sco5944s", "★Sco6630s", "★Sco6027s", "★Sco6247s", "★Sco6252s", "★Sco5928s", "★Sco6241s", "★Sco6165s", "★Tri544s", "★Leo3982s",
			"★Leo4534s", "★Leo4357s", "★Leo4057s", "★Leo4359s", "★Leo4031s", "★Leo3852s", "★Leo3905s", "★Leo3773s", "★Gru8425s", "★Gru8636s",
			"★Gru8353s", "★Lib5685s", "★Lib5531s", "★Lib5787s", "★Lib5603s", "★Pup3165s", "★Pup3185s", "★Pup3045s", "★Cyg7924s", "★Cyg7417s",
			"★Cyg7796s", "★Cyg8301s", "★Cyg7949s", "★Cyg7528s", "★Oct7228s", "★Col1956s", "★Col2040s", "★Col2177s", "★Gem2990s", "★Gem2891s",
			"★Gem2421s", "★Gem2473s", "★Gem2216s", "★Gem2777s", "★Gem2650s", "★Gem2286s", "★Gem2484s", "★Gem2930s", "★Peg8775s", "★Peg8781s",
			"★Peg39s", "★Peg8308s", "★Peg8650s", "★Peg8634s", "★Peg8684s", "★Peg8450s", "★Peg8880s", "★Peg8905s", "★Oph6556s", "★Oph6378s",
			"★Oph6603s", "★Oph6149s", "★Oph6056s", "★Oph6075s", "★Ser5854s", "★Ser7141s", "★Ser5879s", "★Her6406s", "★Her6148s", "★Her6410s",
			"★Her6526s", "★Her6117s", "★Her6008s", "★Per936s", "★Per1017s", "★Per1131s", "★Per1228s", "★Per834s", "★Per941s", "★Phe99s",
			"★Phe338s", "★Vel3634s", "★Vel3485s", "★Vel3734s", "★Aqr8232s", "★Aqr8414s", "★Aqr8709s", "★Aqr8518s", "★Aqr7950s", "★Aqr8499s",
			"★Aqr8610s", "★Aqr8264s", "★Cru4853s", "★Cru4730s", "★Cru4763s", "★Cru4700s", "★Cru4656s", "★PsA8728s", "★TrA6217s", "★Cap7776s",
			"★Cap7754s", "★Cap8278s", "★Cap8322s", "★Cap7773s", "★Sge7479s", "★Car2326s", "★Car3685s", "★Car3307s", "★Car3699s", "★Dra5744s",
			"★Dra5291s", "★Dra6705s", "★Dra6536s", "★Dra7310s", "★Dra6688s", "★Dra4434s", "★Dra6370s", "★Dra7462s", "★Dra6396s", "★Dra6132s",
			"★Dra6636s", "★CVn4915s", "★CVn4785s", "★CVn4846s", "★Aql7595s", "★Aql7557s", "★Aql7525s", "★Aql7602s", "★Aql7235s", "Max Honey",
			"Max Mushrooms", "Galarica Twigs", "Galarica Cuffs", "Style Cards", "Armor Passes", "Rotom Bikes", "Rotom Bikes", "Exp. Charms", "Armorite Ore", "Mark Charms",
			"Reins of Unity", "Reins of Unity", "Galarica Wreaths", "Legendary Clue 1s", "Legendary Clue 2s", "Legendary Clue 3s", "Legendary Clue?s", "Crown Passes", "Wooden Crowns", "Radiant Petals",
			"White Mane Hair", "Black Mane Hair", "Iceroot Carrots", "Shaderoot Carrots", "Dynite Ore", "Carrot Seeds", "Ability Patches", "Reins of Unity"
		});
		this.itemnameplural.Location = new System.Drawing.Point(285, 21);
		this.itemnameplural.Name = "itemnameplural";
		this.itemnameplural.Size = new System.Drawing.Size(102, 21);
		this.itemnameplural.TabIndex = 541;
		this.itemnameplural.Visible = false;
		this.nitem6.Location = new System.Drawing.Point(423, 75);
		this.nitem6.Name = "nitem6";
		this.nitem6.Size = new System.Drawing.Size(41, 20);
		this.nitem6.TabIndex = 12;
		this.nitem5.Location = new System.Drawing.Point(423, 48);
		this.nitem5.Name = "nitem5";
		this.nitem5.Size = new System.Drawing.Size(41, 20);
		this.nitem5.TabIndex = 10;
		this.nitem4.Location = new System.Drawing.Point(423, 21);
		this.nitem4.Name = "nitem4";
		this.nitem4.Size = new System.Drawing.Size(41, 20);
		this.nitem4.TabIndex = 8;
		this.nitem3.Location = new System.Drawing.Point(191, 75);
		this.nitem3.Name = "nitem3";
		this.nitem3.Size = new System.Drawing.Size(41, 20);
		this.nitem3.TabIndex = 6;
		this.nitem2.Location = new System.Drawing.Point(191, 48);
		this.nitem2.Name = "nitem2";
		this.nitem2.Size = new System.Drawing.Size(41, 20);
		this.nitem2.TabIndex = 4;
		this.nitem1.Location = new System.Drawing.Point(191, 21);
		this.nitem1.Name = "nitem1";
		this.nitem1.Size = new System.Drawing.Size(41, 20);
		this.nitem1.TabIndex = 2;
		this.itemslc.Location = new System.Drawing.Point(78, 102);
		this.itemslc.Name = "itemslc";
		this.itemslc.Size = new System.Drawing.Size(154, 20);
		this.itemslc.TabIndex = 13;
		this.Label20.AutoSize = true;
		this.Label20.Location = new System.Drawing.Point(12, 105);
		this.Label20.Name = "Label20";
		this.Label20.Size = new System.Drawing.Size(58, 13);
		this.Label20.TabIndex = 533;
		this.Label20.Text = "Line Count";
		this.Label18.AutoSize = true;
		this.Label18.Location = new System.Drawing.Point(244, 78);
		this.Label18.Name = "Label18";
		this.Label18.Size = new System.Drawing.Size(36, 13);
		this.Label18.TabIndex = 532;
		this.Label18.Text = "Item 6";
		this.Label17.AutoSize = true;
		this.Label17.Location = new System.Drawing.Point(244, 51);
		this.Label17.Name = "Label17";
		this.Label17.Size = new System.Drawing.Size(36, 13);
		this.Label17.TabIndex = 531;
		this.Label17.Text = "Item 5";
		this.Label16.AutoSize = true;
		this.Label16.Location = new System.Drawing.Point(244, 24);
		this.Label16.Name = "Label16";
		this.Label16.Size = new System.Drawing.Size(36, 13);
		this.Label16.TabIndex = 530;
		this.Label16.Text = "Item 4";
		this.Label15.AutoSize = true;
		this.Label15.Location = new System.Drawing.Point(12, 78);
		this.Label15.Name = "Label15";
		this.Label15.Size = new System.Drawing.Size(36, 13);
		this.Label15.TabIndex = 529;
		this.Label15.Text = "Item 3";
		this.Label14.AutoSize = true;
		this.Label14.Location = new System.Drawing.Point(12, 51);
		this.Label14.Name = "Label14";
		this.Label14.Size = new System.Drawing.Size(36, 13);
		this.Label14.TabIndex = 528;
		this.Label14.Text = "Item 2";
		this.itembox6.AllowDrop = true;
		this.itembox6.AutoCompleteCustomSource.AddRange(new string[921]
		{
			"None", "Master Ball", "Ultra Ball", "Great Ball", "Poké Ball", "Safari Ball", "Net Ball", "Dive Ball", "Nest Ball", "Repeat Ball",
			"Timer Ball", "Luxury Ball", "Premier Ball", "Dusk Ball", "Heal Ball", "Quick Ball", "Cherish Ball", "Potion", "Antidote", "Burn Heal",
			"Ice Heal", "Awakening", "Paralyze Heal", "Full Restore", "Max Potion", "Hyper Potion", "Super Potion", "Full Heal", "Revive", "Max Revive",
			"Fresh Water", "Soda Pop", "Lemonade", "Moomoo Milk", "Energy Powder", "Energy Root", "Heal Powder", "Revival Herb", "Ether", "Max Ether",
			"Elixir", "Max Elixir", "Lava Cookie", "Berry Juice", "Sacred Ash", "HP Up", "Protein", "Iron", "Carbos", "Calcium",
			"Rare Candy", "PP Up", "Zinc", "PP Max", "Old Gateau", "Guard Spec.", "Dire Hit", "X Attack", "X Defense", "X Speed",
			"X Accuracy", "X Sp. Atk", "X Sp. Def", "Poké Doll", "Fluffy Tail", "Blue Flute", "Yellow Flute", "Red Flute", "Black Flute", "White Flute",
			"Shoal Salt", "Shoal Shell", "Red Shard", "Blue Shard", "Yellow Shard", "Green Shard", "Super Repel", "Max Repel", "Escape Rope", "Repel",
			"Sun Stone", "Moon Stone", "Fire Stone", "Thunder Stone", "Water Stone", "Leaf Stone", "Tiny Mushroom", "Big Mushroom", "Pearl", "Big Pearl",
			"Stardust", "Star Piece", "Nugget", "Heart Scale", "Honey", "Growth Mulch", "Damp Mulch", "Stable Mulch", "Gooey Mulch", "Root Fossil",
			"Claw Fossil", "Helix Fossil", "Dome Fossil", "Old Amber", "Armor Fossil", "Skull Fossil", "Rare Bone", "Shiny Stone", "Dusk Stone", "Dawn Stone",
			"Oval Stone", "Odd Keystone", "Griseous Orb", "???", "???", "???", "Douse Drive", "Shock Drive", "Burn Drive", "Chill Drive",
			"???", "???", "???", "???", "???", "???", "???", "???", "???", "???",
			"???", "???", "???", "???", "Sweet Heart", "Adamant Orb", "Lustrous Orb", "Greet Mail", "Favored Mail", "RSVP Mail",
			"Thanks Mail", "Inquiry Mail", "Like Mail", "Reply Mail", "Bridge Mail S", "Bridge Mail D", "Bridge Mail T", "Bridge Mail V", "Bridge Mail M", "Cheri Berry",
			"Chesto Berry", "Pecha Berry", "Rawst Berry", "Aspear Berry", "Leppa Berry", "Oran Berry", "Persim Berry", "Lum Berry", "Sitrus Berry", "Figy Berry",
			"Wiki Berry", "Mago Berry", "Aguav Berry", "Iapapa Berry", "Razz Berry", "Bluk Berry", "Nanab Berry", "Wepear Berry", "Pinap Berry", "Pomeg Berry",
			"Kelpsy Berry", "Qualot Berry", "Hondew Berry", "Grepa Berry", "Tamato Berry", "Cornn Berry", "Magost Berry", "Rabuta Berry", "Nomel Berry", "Spelon Berry",
			"Pamtre Berry", "Watmel Berry", "Durin Berry", "Belue Berry", "Occa Berry", "Passho Berry", "Wacan Berry", "Rindo Berry", "Yache Berry", "Chople Berry",
			"Kebia Berry", "Shuca Berry", "Coba Berry", "Payapa Berry", "Tanga Berry", "Charti Berry", "Kasib Berry", "Haban Berry", "Colbur Berry", "Babiri Berry",
			"Chilan Berry", "Liechi Berry", "Ganlon Berry", "Salac Berry", "Petaya Berry", "Apicot Berry", "Lansat Berry", "Starf Berry", "Enigma Berry", "Micle Berry",
			"Custap Berry", "Jaboca Berry", "Rowap Berry", "Bright Powder", "White Herb", "Macho Brace", "Exp. Share", "Quick Claw", "Soothe Bell", "Mental Herb",
			"Choice Band", "King’s Rock", "Silver Powder", "Amulet Coin", "Cleanse Tag", "Soul Dew", "Deep Sea Tooth", "Deep Sea Scale", "Smoke Ball", "Everstone",
			"Focus Band", "Lucky Egg", "Scope Lens", "Metal Coat", "Leftovers", "Dragon Scale", "Light Ball", "Soft Sand", "Hard Stone", "Miracle Seed",
			"Black Glasses", "Black Belt", "Magnet", "Mystic Water", "Sharp Beak", "Poison Barb", "Never-Melt Ice", "Spell Tag", "Twisted Spoon", "Charcoal",
			"Dragon Fang", "Silk Scarf", "Up-Grade", "Shell Bell", "Sea Incense", "Lax Incense", "Lucky Punch", "Metal Powder", "Thick Club", "Stick",
			"Red Scarf", "Blue Scarf", "Pink Scarf", "Green Scarf", "Yellow Scarf", "Wide Lens", "Muscle Band", "Wise Glasses", "Expert Belt", "Light Clay",
			"Life Orb", "Power Herb", "Toxic Orb", "Flame Orb", "Quick Powder", "Focus Sash", "Zoom Lens", "Metronome", "Iron Ball", "Lagging Tail",
			"Destiny Knot", "Black Sludge", "Icy Rock", "Smooth Rock", "Heat Rock", "Damp Rock", "Grip Claw", "Choice Scarf", "Sticky Barb", "Power Bracer",
			"Power Belt", "Power Lens", "Power Band", "Power Anklet", "Power Weight", "Shed Shell", "Big Root", "Choice Specs", "Flame Plate", "Splash Plate",
			"Zap Plate", "Meadow Plate", "Icicle Plate", "Fist Plate", "Toxic Plate", "Earth Plate", "Sky Plate", "Mind Plate", "Insect Plate", "Stone Plate",
			"Spooky Plate", "Draco Plate", "Dread Plate", "Iron Plate", "Odd Incense", "Rock Incense", "Full Incense", "Wave Incense", "Rose Incense", "Luck Incense",
			"Pure Incense", "Protector", "Electirizer", "Magmarizer", "Dubious Disc", "Reaper Cloth", "Razor Claw", "Razor Fang", "TM01", "TM02",
			"TM03", "TM04", "TM05", "TM06", "TM07", "TM08", "TM09", "TM10", "TM11", "TM12",
			"TM13", "TM14", "TM15", "TM16", "TM17", "TM18", "TM19", "TM20", "TM21", "TM22",
			"TM23", "TM24", "TM25", "TM26", "TM27", "TM28", "TM29", "TM30", "TM31", "TM32",
			"TM33", "TM34", "TM35", "TM36", "TM37", "TM38", "TM39", "TM40", "TM41", "TM42",
			"TM43", "TM44", "TM45", "TM46", "TM47", "TM48", "TM49", "TM50", "TM51", "TM52",
			"TM53", "TM54", "TM55", "TM56", "TM57", "TM58", "TM59", "TM60", "TM61", "TM62",
			"TM63", "TM64", "TM65", "TM66", "TM67", "TM68", "TM69", "TM70", "TM71", "TM72",
			"TM73", "TM74", "TM75", "TM76", "TM77", "TM78", "TM79", "TM80", "TM81", "TM82",
			"TM83", "TM84", "TM85", "TM86", "TM87", "TM88", "TM89", "TM90", "TM91", "TM92",
			"HM01", "HM02", "HM03", "HM04", "HM05", "HM06", "???", "???", "Explorer Kit", "Loot Sack",
			"Rule Book", "Poké Radar", "Point Card", "Journal", "Seal Case", "Fashion Case", "Seal Bag", "Pal Pad", "Works Key", "Old Charm",
			"Galactic Key", "Red Chain", "Town Map", "Vs. Seeker", "Coin Case", "Old Rod", "Good Rod", "Super Rod", "Sprayduck", "Poffin Case",
			"Bike", "Suite Key", "Oak’s Letter", "Lunar Wing", "Member Card", "Azure Flute", "S.S. Ticket", "Contest Pass", "Magma Stone", "Parcel",
			"Coupon 1", "Coupon 2", "Coupon 3", "Storage Key", "Secret Potion", "Vs. Recorder", "Gracidea", "Secret Key", "Apricorn Box", "Unown Report",
			"Berry Pots", "Dowsing Machine", "Blue Card", "Slowpoke Tail", "Clear Bell", "Card Key", "Basement Key", "Squirt Bottle", "Red Scale", "Lost Item",
			"Pass", "Machine Part", "Silver Wing", "Rainbow Wing", "Mystery Egg", "Red Apricorn", "Blue Apricorn", "Yellow Apricorn", "Green Apricorn", "Pink Apricorn",
			"White Apricorn", "Black Apricorn", "Fast Ball", "Level Ball", "Lure Ball", "Heavy Ball", "Love Ball", "Friend Ball", "Moon Ball", "Sport Ball",
			"Park Ball", "Photo Album", "GB Sounds", "Tidal Bell", "Rage Candy Bar", "Data Card 01", "Data Card 02", "Data Card 03", "Data Card 04", "Data Card 05",
			"Data Card 06", "Data Card 07", "Data Card 08", "Data Card 09", "Data Card 10", "Data Card 11", "Data Card 12", "Data Card 13", "Data Card 14", "Data Card 15",
			"Data Card 16", "Data Card 17", "Data Card 18", "Data Card 19", "Data Card 20", "Data Card 21", "Data Card 22", "Data Card 23", "Data Card 24", "Data Card 25",
			"Data Card 26", "Data Card 27", "Jade Orb", "Lock Capsule", "Red Orb", "Blue Orb", "Enigma Stone", "Prism Scale", "Eviolite", "Float Stone",
			"Rocky Helmet", "Air Balloon", "Red Card", "Ring Target", "Binding Band", "Absorb Bulb", "Cell Battery", "Eject Button", "Fire Gem", "Water Gem",
			"Electric Gem", "Grass Gem", "Ice Gem", "Fighting Gem", "Poison Gem", "Ground Gem", "Flying Gem", "Psychic Gem", "Bug Gem", "Rock Gem",
			"Ghost Gem", "Dragon Gem", "Dark Gem", "Steel Gem", "Normal Gem", "Health Wing", "Muscle Wing", "Resist Wing", "Genius Wing", "Clever Wing",
			"Swift Wing", "Pretty Wing", "Cover Fossil", "Plume Fossil", "Liberty Pass", "Pass Orb", "Dream Ball", "Poké Toy", "Prop Case", "Dragon Skull",
			"Balm Mushroom", "Big Nugget", "Pearl String", "Comet Shard", "Relic Copper", "Relic Silver", "Relic Gold", "Relic Vase", "Relic Band", "Relic Statue",
			"Relic Crown", "Casteliacone", "Dire Hit 2", "X Speed 2", "X Sp. Atk 2", "X Sp. Def 2", "X Defense 2", "X Attack 2", "X Accuracy 2", "X Speed 3",
			"X Sp. Atk 3", "X Sp. Def 3", "X Defense 3", "X Attack 3", "X Accuracy 3", "X Speed 6", "X Sp. Atk 6", "X Sp. Def 6", "X Defense 6", "X Attack 6",
			"X Accuracy 6", "Ability Urge", "Item Drop", "Item Urge", "Reset Urge", "Dire Hit 3", "Light Stone", "Dark Stone", "TM93", "TM94",
			"TM95", "Xtransceiver", "???", "Gram 1", "Gram 2", "Gram 3", "Xtransceiver", "Medal Box", "DNA Splicers", "DNA Splicers",
			"Permit", "Oval Charm", "Shiny Charm", "Plasma Card", "Grubby Hanky", "Colress Machine", "Dropped Item", "Dropped Item", "Reveal Glass", "Weakness Policy",
			"Assault Vest", "Holo Caster", "Prof’s Letter", "Roller Skates", "Pixie Plate", "Ability Capsule", "Whipped Dream", "Sachet", "Luminous Moss", "Snowball",
			"Safety Goggles", "Poké Flute", "Rich Mulch", "Surprise Mulch", "Boost Mulch", "Amaze Mulch", "Gengarite", "Gardevoirite", "Ampharosite", "Venusaurite",
			"Charizardite X", "Blastoisinite", "Mewtwonite X", "Mewtwonite Y", "Blazikenite", "Medichamite", "Houndoominite", "Aggronite", "Banettite", "Tyranitarite",
			"Scizorite", "Pinsirite", "Aerodactylite", "Lucarionite", "Abomasite", "Kangaskhanite", "Gyaradosite", "Absolite", "Charizardite Y", "Alakazite",
			"Heracronite", "Mawilite", "Manectite", "Garchompite", "Latiasite", "Latiosite", "Roseli Berry", "Kee Berry", "Maranga Berry", "Sprinklotad",
			"TM96", "TM97", "TM98", "TM99", "TM100", "Power Plant Pass", "Mega Ring", "Intriguing Stone", "Common Stone", "Discount Coupon",
			"Elevator Key", "TMV Pass", "Honor of Kalos", "Adventure Rules", "Strange Souvenir", "Lens Case", "Makeup Bag", "Travel Trunk", "Lumiose Galette", "Shalour Sable",
			"Jaw Fossil", "Sail Fossil", "Looker Ticket", "Bike", "Holo Caster", "Fairy Gem", "Mega Charm", "Mega Glove", "Mach Bike", "Acro Bike",
			"Wailmer Pail", "Devon Parts", "Soot Sack", "Basement Key", "Pokéblock Kit", "Letter", "Eon Ticket", "Scanner", "Go-Goggles", "Meteorite",
			"Key to Room 1", "Key to Room 2", "Key to Room 4", "Key to Room 6", "Storage Key", "Devon Scope", "S.S. Ticket", "HM07", "Devon Scuba Gear", "Contest Costume",
			"Contest Costume", "Magma Suit", "Aqua Suit", "Pair of Tickets", "Mega Bracelet", "Mega Pendant", "Mega Glasses", "Mega Anchor", "Mega Stickpin", "Mega Tiara",
			"Mega Anklet", "Meteorite", "Swampertite", "Sceptilite", "Sablenite", "Altarianite", "Galladite", "Audinite", "Metagrossite", "Sharpedonite",
			"Slowbronite", "Steelixite", "Pidgeotite", "Glalitite", "Diancite", "Prison Bottle", "Mega Cuff", "Cameruptite", "Lopunnite", "Salamencite",
			"Beedrillite", "Meteorite", "Meteorite", "Key Stone", "Meteorite Shard", "Eon Flute", "n/a", "n/a", "n/a", "Electrium Z",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "Z-Ring", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"Electrium Z", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "UB Ball *", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a"
		});
		this.itembox6.FormattingEnabled = true;
		this.itembox6.Location = new System.Drawing.Point(310, 75);
		this.itembox6.Name = "itembox6";
		this.itembox6.Size = new System.Drawing.Size(107, 21);
		this.itembox6.TabIndex = 11;
		this.itembox5.AllowDrop = true;
		this.itembox5.AutoCompleteCustomSource.AddRange(new string[921]
		{
			"None", "Master Ball", "Ultra Ball", "Great Ball", "Poké Ball", "Safari Ball", "Net Ball", "Dive Ball", "Nest Ball", "Repeat Ball",
			"Timer Ball", "Luxury Ball", "Premier Ball", "Dusk Ball", "Heal Ball", "Quick Ball", "Cherish Ball", "Potion", "Antidote", "Burn Heal",
			"Ice Heal", "Awakening", "Paralyze Heal", "Full Restore", "Max Potion", "Hyper Potion", "Super Potion", "Full Heal", "Revive", "Max Revive",
			"Fresh Water", "Soda Pop", "Lemonade", "Moomoo Milk", "Energy Powder", "Energy Root", "Heal Powder", "Revival Herb", "Ether", "Max Ether",
			"Elixir", "Max Elixir", "Lava Cookie", "Berry Juice", "Sacred Ash", "HP Up", "Protein", "Iron", "Carbos", "Calcium",
			"Rare Candy", "PP Up", "Zinc", "PP Max", "Old Gateau", "Guard Spec.", "Dire Hit", "X Attack", "X Defense", "X Speed",
			"X Accuracy", "X Sp. Atk", "X Sp. Def", "Poké Doll", "Fluffy Tail", "Blue Flute", "Yellow Flute", "Red Flute", "Black Flute", "White Flute",
			"Shoal Salt", "Shoal Shell", "Red Shard", "Blue Shard", "Yellow Shard", "Green Shard", "Super Repel", "Max Repel", "Escape Rope", "Repel",
			"Sun Stone", "Moon Stone", "Fire Stone", "Thunder Stone", "Water Stone", "Leaf Stone", "Tiny Mushroom", "Big Mushroom", "Pearl", "Big Pearl",
			"Stardust", "Star Piece", "Nugget", "Heart Scale", "Honey", "Growth Mulch", "Damp Mulch", "Stable Mulch", "Gooey Mulch", "Root Fossil",
			"Claw Fossil", "Helix Fossil", "Dome Fossil", "Old Amber", "Armor Fossil", "Skull Fossil", "Rare Bone", "Shiny Stone", "Dusk Stone", "Dawn Stone",
			"Oval Stone", "Odd Keystone", "Griseous Orb", "???", "???", "???", "Douse Drive", "Shock Drive", "Burn Drive", "Chill Drive",
			"???", "???", "???", "???", "???", "???", "???", "???", "???", "???",
			"???", "???", "???", "???", "Sweet Heart", "Adamant Orb", "Lustrous Orb", "Greet Mail", "Favored Mail", "RSVP Mail",
			"Thanks Mail", "Inquiry Mail", "Like Mail", "Reply Mail", "Bridge Mail S", "Bridge Mail D", "Bridge Mail T", "Bridge Mail V", "Bridge Mail M", "Cheri Berry",
			"Chesto Berry", "Pecha Berry", "Rawst Berry", "Aspear Berry", "Leppa Berry", "Oran Berry", "Persim Berry", "Lum Berry", "Sitrus Berry", "Figy Berry",
			"Wiki Berry", "Mago Berry", "Aguav Berry", "Iapapa Berry", "Razz Berry", "Bluk Berry", "Nanab Berry", "Wepear Berry", "Pinap Berry", "Pomeg Berry",
			"Kelpsy Berry", "Qualot Berry", "Hondew Berry", "Grepa Berry", "Tamato Berry", "Cornn Berry", "Magost Berry", "Rabuta Berry", "Nomel Berry", "Spelon Berry",
			"Pamtre Berry", "Watmel Berry", "Durin Berry", "Belue Berry", "Occa Berry", "Passho Berry", "Wacan Berry", "Rindo Berry", "Yache Berry", "Chople Berry",
			"Kebia Berry", "Shuca Berry", "Coba Berry", "Payapa Berry", "Tanga Berry", "Charti Berry", "Kasib Berry", "Haban Berry", "Colbur Berry", "Babiri Berry",
			"Chilan Berry", "Liechi Berry", "Ganlon Berry", "Salac Berry", "Petaya Berry", "Apicot Berry", "Lansat Berry", "Starf Berry", "Enigma Berry", "Micle Berry",
			"Custap Berry", "Jaboca Berry", "Rowap Berry", "Bright Powder", "White Herb", "Macho Brace", "Exp. Share", "Quick Claw", "Soothe Bell", "Mental Herb",
			"Choice Band", "King’s Rock", "Silver Powder", "Amulet Coin", "Cleanse Tag", "Soul Dew", "Deep Sea Tooth", "Deep Sea Scale", "Smoke Ball", "Everstone",
			"Focus Band", "Lucky Egg", "Scope Lens", "Metal Coat", "Leftovers", "Dragon Scale", "Light Ball", "Soft Sand", "Hard Stone", "Miracle Seed",
			"Black Glasses", "Black Belt", "Magnet", "Mystic Water", "Sharp Beak", "Poison Barb", "Never-Melt Ice", "Spell Tag", "Twisted Spoon", "Charcoal",
			"Dragon Fang", "Silk Scarf", "Up-Grade", "Shell Bell", "Sea Incense", "Lax Incense", "Lucky Punch", "Metal Powder", "Thick Club", "Stick",
			"Red Scarf", "Blue Scarf", "Pink Scarf", "Green Scarf", "Yellow Scarf", "Wide Lens", "Muscle Band", "Wise Glasses", "Expert Belt", "Light Clay",
			"Life Orb", "Power Herb", "Toxic Orb", "Flame Orb", "Quick Powder", "Focus Sash", "Zoom Lens", "Metronome", "Iron Ball", "Lagging Tail",
			"Destiny Knot", "Black Sludge", "Icy Rock", "Smooth Rock", "Heat Rock", "Damp Rock", "Grip Claw", "Choice Scarf", "Sticky Barb", "Power Bracer",
			"Power Belt", "Power Lens", "Power Band", "Power Anklet", "Power Weight", "Shed Shell", "Big Root", "Choice Specs", "Flame Plate", "Splash Plate",
			"Zap Plate", "Meadow Plate", "Icicle Plate", "Fist Plate", "Toxic Plate", "Earth Plate", "Sky Plate", "Mind Plate", "Insect Plate", "Stone Plate",
			"Spooky Plate", "Draco Plate", "Dread Plate", "Iron Plate", "Odd Incense", "Rock Incense", "Full Incense", "Wave Incense", "Rose Incense", "Luck Incense",
			"Pure Incense", "Protector", "Electirizer", "Magmarizer", "Dubious Disc", "Reaper Cloth", "Razor Claw", "Razor Fang", "TM01", "TM02",
			"TM03", "TM04", "TM05", "TM06", "TM07", "TM08", "TM09", "TM10", "TM11", "TM12",
			"TM13", "TM14", "TM15", "TM16", "TM17", "TM18", "TM19", "TM20", "TM21", "TM22",
			"TM23", "TM24", "TM25", "TM26", "TM27", "TM28", "TM29", "TM30", "TM31", "TM32",
			"TM33", "TM34", "TM35", "TM36", "TM37", "TM38", "TM39", "TM40", "TM41", "TM42",
			"TM43", "TM44", "TM45", "TM46", "TM47", "TM48", "TM49", "TM50", "TM51", "TM52",
			"TM53", "TM54", "TM55", "TM56", "TM57", "TM58", "TM59", "TM60", "TM61", "TM62",
			"TM63", "TM64", "TM65", "TM66", "TM67", "TM68", "TM69", "TM70", "TM71", "TM72",
			"TM73", "TM74", "TM75", "TM76", "TM77", "TM78", "TM79", "TM80", "TM81", "TM82",
			"TM83", "TM84", "TM85", "TM86", "TM87", "TM88", "TM89", "TM90", "TM91", "TM92",
			"HM01", "HM02", "HM03", "HM04", "HM05", "HM06", "???", "???", "Explorer Kit", "Loot Sack",
			"Rule Book", "Poké Radar", "Point Card", "Journal", "Seal Case", "Fashion Case", "Seal Bag", "Pal Pad", "Works Key", "Old Charm",
			"Galactic Key", "Red Chain", "Town Map", "Vs. Seeker", "Coin Case", "Old Rod", "Good Rod", "Super Rod", "Sprayduck", "Poffin Case",
			"Bike", "Suite Key", "Oak’s Letter", "Lunar Wing", "Member Card", "Azure Flute", "S.S. Ticket", "Contest Pass", "Magma Stone", "Parcel",
			"Coupon 1", "Coupon 2", "Coupon 3", "Storage Key", "Secret Potion", "Vs. Recorder", "Gracidea", "Secret Key", "Apricorn Box", "Unown Report",
			"Berry Pots", "Dowsing Machine", "Blue Card", "Slowpoke Tail", "Clear Bell", "Card Key", "Basement Key", "Squirt Bottle", "Red Scale", "Lost Item",
			"Pass", "Machine Part", "Silver Wing", "Rainbow Wing", "Mystery Egg", "Red Apricorn", "Blue Apricorn", "Yellow Apricorn", "Green Apricorn", "Pink Apricorn",
			"White Apricorn", "Black Apricorn", "Fast Ball", "Level Ball", "Lure Ball", "Heavy Ball", "Love Ball", "Friend Ball", "Moon Ball", "Sport Ball",
			"Park Ball", "Photo Album", "GB Sounds", "Tidal Bell", "Rage Candy Bar", "Data Card 01", "Data Card 02", "Data Card 03", "Data Card 04", "Data Card 05",
			"Data Card 06", "Data Card 07", "Data Card 08", "Data Card 09", "Data Card 10", "Data Card 11", "Data Card 12", "Data Card 13", "Data Card 14", "Data Card 15",
			"Data Card 16", "Data Card 17", "Data Card 18", "Data Card 19", "Data Card 20", "Data Card 21", "Data Card 22", "Data Card 23", "Data Card 24", "Data Card 25",
			"Data Card 26", "Data Card 27", "Jade Orb", "Lock Capsule", "Red Orb", "Blue Orb", "Enigma Stone", "Prism Scale", "Eviolite", "Float Stone",
			"Rocky Helmet", "Air Balloon", "Red Card", "Ring Target", "Binding Band", "Absorb Bulb", "Cell Battery", "Eject Button", "Fire Gem", "Water Gem",
			"Electric Gem", "Grass Gem", "Ice Gem", "Fighting Gem", "Poison Gem", "Ground Gem", "Flying Gem", "Psychic Gem", "Bug Gem", "Rock Gem",
			"Ghost Gem", "Dragon Gem", "Dark Gem", "Steel Gem", "Normal Gem", "Health Wing", "Muscle Wing", "Resist Wing", "Genius Wing", "Clever Wing",
			"Swift Wing", "Pretty Wing", "Cover Fossil", "Plume Fossil", "Liberty Pass", "Pass Orb", "Dream Ball", "Poké Toy", "Prop Case", "Dragon Skull",
			"Balm Mushroom", "Big Nugget", "Pearl String", "Comet Shard", "Relic Copper", "Relic Silver", "Relic Gold", "Relic Vase", "Relic Band", "Relic Statue",
			"Relic Crown", "Casteliacone", "Dire Hit 2", "X Speed 2", "X Sp. Atk 2", "X Sp. Def 2", "X Defense 2", "X Attack 2", "X Accuracy 2", "X Speed 3",
			"X Sp. Atk 3", "X Sp. Def 3", "X Defense 3", "X Attack 3", "X Accuracy 3", "X Speed 6", "X Sp. Atk 6", "X Sp. Def 6", "X Defense 6", "X Attack 6",
			"X Accuracy 6", "Ability Urge", "Item Drop", "Item Urge", "Reset Urge", "Dire Hit 3", "Light Stone", "Dark Stone", "TM93", "TM94",
			"TM95", "Xtransceiver", "???", "Gram 1", "Gram 2", "Gram 3", "Xtransceiver", "Medal Box", "DNA Splicers", "DNA Splicers",
			"Permit", "Oval Charm", "Shiny Charm", "Plasma Card", "Grubby Hanky", "Colress Machine", "Dropped Item", "Dropped Item", "Reveal Glass", "Weakness Policy",
			"Assault Vest", "Holo Caster", "Prof’s Letter", "Roller Skates", "Pixie Plate", "Ability Capsule", "Whipped Dream", "Sachet", "Luminous Moss", "Snowball",
			"Safety Goggles", "Poké Flute", "Rich Mulch", "Surprise Mulch", "Boost Mulch", "Amaze Mulch", "Gengarite", "Gardevoirite", "Ampharosite", "Venusaurite",
			"Charizardite X", "Blastoisinite", "Mewtwonite X", "Mewtwonite Y", "Blazikenite", "Medichamite", "Houndoominite", "Aggronite", "Banettite", "Tyranitarite",
			"Scizorite", "Pinsirite", "Aerodactylite", "Lucarionite", "Abomasite", "Kangaskhanite", "Gyaradosite", "Absolite", "Charizardite Y", "Alakazite",
			"Heracronite", "Mawilite", "Manectite", "Garchompite", "Latiasite", "Latiosite", "Roseli Berry", "Kee Berry", "Maranga Berry", "Sprinklotad",
			"TM96", "TM97", "TM98", "TM99", "TM100", "Power Plant Pass", "Mega Ring", "Intriguing Stone", "Common Stone", "Discount Coupon",
			"Elevator Key", "TMV Pass", "Honor of Kalos", "Adventure Rules", "Strange Souvenir", "Lens Case", "Makeup Bag", "Travel Trunk", "Lumiose Galette", "Shalour Sable",
			"Jaw Fossil", "Sail Fossil", "Looker Ticket", "Bike", "Holo Caster", "Fairy Gem", "Mega Charm", "Mega Glove", "Mach Bike", "Acro Bike",
			"Wailmer Pail", "Devon Parts", "Soot Sack", "Basement Key", "Pokéblock Kit", "Letter", "Eon Ticket", "Scanner", "Go-Goggles", "Meteorite",
			"Key to Room 1", "Key to Room 2", "Key to Room 4", "Key to Room 6", "Storage Key", "Devon Scope", "S.S. Ticket", "HM07", "Devon Scuba Gear", "Contest Costume",
			"Contest Costume", "Magma Suit", "Aqua Suit", "Pair of Tickets", "Mega Bracelet", "Mega Pendant", "Mega Glasses", "Mega Anchor", "Mega Stickpin", "Mega Tiara",
			"Mega Anklet", "Meteorite", "Swampertite", "Sceptilite", "Sablenite", "Altarianite", "Galladite", "Audinite", "Metagrossite", "Sharpedonite",
			"Slowbronite", "Steelixite", "Pidgeotite", "Glalitite", "Diancite", "Prison Bottle", "Mega Cuff", "Cameruptite", "Lopunnite", "Salamencite",
			"Beedrillite", "Meteorite", "Meteorite", "Key Stone", "Meteorite Shard", "Eon Flute", "n/a", "n/a", "n/a", "Electrium Z",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "Z-Ring", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"Electrium Z", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "UB Ball *", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a"
		});
		this.itembox5.FormattingEnabled = true;
		this.itembox5.Location = new System.Drawing.Point(310, 48);
		this.itembox5.Name = "itembox5";
		this.itembox5.Size = new System.Drawing.Size(107, 21);
		this.itembox5.TabIndex = 9;
		this.itembox4.AllowDrop = true;
		this.itembox4.AutoCompleteCustomSource.AddRange(new string[921]
		{
			"None", "Master Ball", "Ultra Ball", "Great Ball", "Poké Ball", "Safari Ball", "Net Ball", "Dive Ball", "Nest Ball", "Repeat Ball",
			"Timer Ball", "Luxury Ball", "Premier Ball", "Dusk Ball", "Heal Ball", "Quick Ball", "Cherish Ball", "Potion", "Antidote", "Burn Heal",
			"Ice Heal", "Awakening", "Paralyze Heal", "Full Restore", "Max Potion", "Hyper Potion", "Super Potion", "Full Heal", "Revive", "Max Revive",
			"Fresh Water", "Soda Pop", "Lemonade", "Moomoo Milk", "Energy Powder", "Energy Root", "Heal Powder", "Revival Herb", "Ether", "Max Ether",
			"Elixir", "Max Elixir", "Lava Cookie", "Berry Juice", "Sacred Ash", "HP Up", "Protein", "Iron", "Carbos", "Calcium",
			"Rare Candy", "PP Up", "Zinc", "PP Max", "Old Gateau", "Guard Spec.", "Dire Hit", "X Attack", "X Defense", "X Speed",
			"X Accuracy", "X Sp. Atk", "X Sp. Def", "Poké Doll", "Fluffy Tail", "Blue Flute", "Yellow Flute", "Red Flute", "Black Flute", "White Flute",
			"Shoal Salt", "Shoal Shell", "Red Shard", "Blue Shard", "Yellow Shard", "Green Shard", "Super Repel", "Max Repel", "Escape Rope", "Repel",
			"Sun Stone", "Moon Stone", "Fire Stone", "Thunder Stone", "Water Stone", "Leaf Stone", "Tiny Mushroom", "Big Mushroom", "Pearl", "Big Pearl",
			"Stardust", "Star Piece", "Nugget", "Heart Scale", "Honey", "Growth Mulch", "Damp Mulch", "Stable Mulch", "Gooey Mulch", "Root Fossil",
			"Claw Fossil", "Helix Fossil", "Dome Fossil", "Old Amber", "Armor Fossil", "Skull Fossil", "Rare Bone", "Shiny Stone", "Dusk Stone", "Dawn Stone",
			"Oval Stone", "Odd Keystone", "Griseous Orb", "???", "???", "???", "Douse Drive", "Shock Drive", "Burn Drive", "Chill Drive",
			"???", "???", "???", "???", "???", "???", "???", "???", "???", "???",
			"???", "???", "???", "???", "Sweet Heart", "Adamant Orb", "Lustrous Orb", "Greet Mail", "Favored Mail", "RSVP Mail",
			"Thanks Mail", "Inquiry Mail", "Like Mail", "Reply Mail", "Bridge Mail S", "Bridge Mail D", "Bridge Mail T", "Bridge Mail V", "Bridge Mail M", "Cheri Berry",
			"Chesto Berry", "Pecha Berry", "Rawst Berry", "Aspear Berry", "Leppa Berry", "Oran Berry", "Persim Berry", "Lum Berry", "Sitrus Berry", "Figy Berry",
			"Wiki Berry", "Mago Berry", "Aguav Berry", "Iapapa Berry", "Razz Berry", "Bluk Berry", "Nanab Berry", "Wepear Berry", "Pinap Berry", "Pomeg Berry",
			"Kelpsy Berry", "Qualot Berry", "Hondew Berry", "Grepa Berry", "Tamato Berry", "Cornn Berry", "Magost Berry", "Rabuta Berry", "Nomel Berry", "Spelon Berry",
			"Pamtre Berry", "Watmel Berry", "Durin Berry", "Belue Berry", "Occa Berry", "Passho Berry", "Wacan Berry", "Rindo Berry", "Yache Berry", "Chople Berry",
			"Kebia Berry", "Shuca Berry", "Coba Berry", "Payapa Berry", "Tanga Berry", "Charti Berry", "Kasib Berry", "Haban Berry", "Colbur Berry", "Babiri Berry",
			"Chilan Berry", "Liechi Berry", "Ganlon Berry", "Salac Berry", "Petaya Berry", "Apicot Berry", "Lansat Berry", "Starf Berry", "Enigma Berry", "Micle Berry",
			"Custap Berry", "Jaboca Berry", "Rowap Berry", "Bright Powder", "White Herb", "Macho Brace", "Exp. Share", "Quick Claw", "Soothe Bell", "Mental Herb",
			"Choice Band", "King’s Rock", "Silver Powder", "Amulet Coin", "Cleanse Tag", "Soul Dew", "Deep Sea Tooth", "Deep Sea Scale", "Smoke Ball", "Everstone",
			"Focus Band", "Lucky Egg", "Scope Lens", "Metal Coat", "Leftovers", "Dragon Scale", "Light Ball", "Soft Sand", "Hard Stone", "Miracle Seed",
			"Black Glasses", "Black Belt", "Magnet", "Mystic Water", "Sharp Beak", "Poison Barb", "Never-Melt Ice", "Spell Tag", "Twisted Spoon", "Charcoal",
			"Dragon Fang", "Silk Scarf", "Up-Grade", "Shell Bell", "Sea Incense", "Lax Incense", "Lucky Punch", "Metal Powder", "Thick Club", "Stick",
			"Red Scarf", "Blue Scarf", "Pink Scarf", "Green Scarf", "Yellow Scarf", "Wide Lens", "Muscle Band", "Wise Glasses", "Expert Belt", "Light Clay",
			"Life Orb", "Power Herb", "Toxic Orb", "Flame Orb", "Quick Powder", "Focus Sash", "Zoom Lens", "Metronome", "Iron Ball", "Lagging Tail",
			"Destiny Knot", "Black Sludge", "Icy Rock", "Smooth Rock", "Heat Rock", "Damp Rock", "Grip Claw", "Choice Scarf", "Sticky Barb", "Power Bracer",
			"Power Belt", "Power Lens", "Power Band", "Power Anklet", "Power Weight", "Shed Shell", "Big Root", "Choice Specs", "Flame Plate", "Splash Plate",
			"Zap Plate", "Meadow Plate", "Icicle Plate", "Fist Plate", "Toxic Plate", "Earth Plate", "Sky Plate", "Mind Plate", "Insect Plate", "Stone Plate",
			"Spooky Plate", "Draco Plate", "Dread Plate", "Iron Plate", "Odd Incense", "Rock Incense", "Full Incense", "Wave Incense", "Rose Incense", "Luck Incense",
			"Pure Incense", "Protector", "Electirizer", "Magmarizer", "Dubious Disc", "Reaper Cloth", "Razor Claw", "Razor Fang", "TM01", "TM02",
			"TM03", "TM04", "TM05", "TM06", "TM07", "TM08", "TM09", "TM10", "TM11", "TM12",
			"TM13", "TM14", "TM15", "TM16", "TM17", "TM18", "TM19", "TM20", "TM21", "TM22",
			"TM23", "TM24", "TM25", "TM26", "TM27", "TM28", "TM29", "TM30", "TM31", "TM32",
			"TM33", "TM34", "TM35", "TM36", "TM37", "TM38", "TM39", "TM40", "TM41", "TM42",
			"TM43", "TM44", "TM45", "TM46", "TM47", "TM48", "TM49", "TM50", "TM51", "TM52",
			"TM53", "TM54", "TM55", "TM56", "TM57", "TM58", "TM59", "TM60", "TM61", "TM62",
			"TM63", "TM64", "TM65", "TM66", "TM67", "TM68", "TM69", "TM70", "TM71", "TM72",
			"TM73", "TM74", "TM75", "TM76", "TM77", "TM78", "TM79", "TM80", "TM81", "TM82",
			"TM83", "TM84", "TM85", "TM86", "TM87", "TM88", "TM89", "TM90", "TM91", "TM92",
			"HM01", "HM02", "HM03", "HM04", "HM05", "HM06", "???", "???", "Explorer Kit", "Loot Sack",
			"Rule Book", "Poké Radar", "Point Card", "Journal", "Seal Case", "Fashion Case", "Seal Bag", "Pal Pad", "Works Key", "Old Charm",
			"Galactic Key", "Red Chain", "Town Map", "Vs. Seeker", "Coin Case", "Old Rod", "Good Rod", "Super Rod", "Sprayduck", "Poffin Case",
			"Bike", "Suite Key", "Oak’s Letter", "Lunar Wing", "Member Card", "Azure Flute", "S.S. Ticket", "Contest Pass", "Magma Stone", "Parcel",
			"Coupon 1", "Coupon 2", "Coupon 3", "Storage Key", "Secret Potion", "Vs. Recorder", "Gracidea", "Secret Key", "Apricorn Box", "Unown Report",
			"Berry Pots", "Dowsing Machine", "Blue Card", "Slowpoke Tail", "Clear Bell", "Card Key", "Basement Key", "Squirt Bottle", "Red Scale", "Lost Item",
			"Pass", "Machine Part", "Silver Wing", "Rainbow Wing", "Mystery Egg", "Red Apricorn", "Blue Apricorn", "Yellow Apricorn", "Green Apricorn", "Pink Apricorn",
			"White Apricorn", "Black Apricorn", "Fast Ball", "Level Ball", "Lure Ball", "Heavy Ball", "Love Ball", "Friend Ball", "Moon Ball", "Sport Ball",
			"Park Ball", "Photo Album", "GB Sounds", "Tidal Bell", "Rage Candy Bar", "Data Card 01", "Data Card 02", "Data Card 03", "Data Card 04", "Data Card 05",
			"Data Card 06", "Data Card 07", "Data Card 08", "Data Card 09", "Data Card 10", "Data Card 11", "Data Card 12", "Data Card 13", "Data Card 14", "Data Card 15",
			"Data Card 16", "Data Card 17", "Data Card 18", "Data Card 19", "Data Card 20", "Data Card 21", "Data Card 22", "Data Card 23", "Data Card 24", "Data Card 25",
			"Data Card 26", "Data Card 27", "Jade Orb", "Lock Capsule", "Red Orb", "Blue Orb", "Enigma Stone", "Prism Scale", "Eviolite", "Float Stone",
			"Rocky Helmet", "Air Balloon", "Red Card", "Ring Target", "Binding Band", "Absorb Bulb", "Cell Battery", "Eject Button", "Fire Gem", "Water Gem",
			"Electric Gem", "Grass Gem", "Ice Gem", "Fighting Gem", "Poison Gem", "Ground Gem", "Flying Gem", "Psychic Gem", "Bug Gem", "Rock Gem",
			"Ghost Gem", "Dragon Gem", "Dark Gem", "Steel Gem", "Normal Gem", "Health Wing", "Muscle Wing", "Resist Wing", "Genius Wing", "Clever Wing",
			"Swift Wing", "Pretty Wing", "Cover Fossil", "Plume Fossil", "Liberty Pass", "Pass Orb", "Dream Ball", "Poké Toy", "Prop Case", "Dragon Skull",
			"Balm Mushroom", "Big Nugget", "Pearl String", "Comet Shard", "Relic Copper", "Relic Silver", "Relic Gold", "Relic Vase", "Relic Band", "Relic Statue",
			"Relic Crown", "Casteliacone", "Dire Hit 2", "X Speed 2", "X Sp. Atk 2", "X Sp. Def 2", "X Defense 2", "X Attack 2", "X Accuracy 2", "X Speed 3",
			"X Sp. Atk 3", "X Sp. Def 3", "X Defense 3", "X Attack 3", "X Accuracy 3", "X Speed 6", "X Sp. Atk 6", "X Sp. Def 6", "X Defense 6", "X Attack 6",
			"X Accuracy 6", "Ability Urge", "Item Drop", "Item Urge", "Reset Urge", "Dire Hit 3", "Light Stone", "Dark Stone", "TM93", "TM94",
			"TM95", "Xtransceiver", "???", "Gram 1", "Gram 2", "Gram 3", "Xtransceiver", "Medal Box", "DNA Splicers", "DNA Splicers",
			"Permit", "Oval Charm", "Shiny Charm", "Plasma Card", "Grubby Hanky", "Colress Machine", "Dropped Item", "Dropped Item", "Reveal Glass", "Weakness Policy",
			"Assault Vest", "Holo Caster", "Prof’s Letter", "Roller Skates", "Pixie Plate", "Ability Capsule", "Whipped Dream", "Sachet", "Luminous Moss", "Snowball",
			"Safety Goggles", "Poké Flute", "Rich Mulch", "Surprise Mulch", "Boost Mulch", "Amaze Mulch", "Gengarite", "Gardevoirite", "Ampharosite", "Venusaurite",
			"Charizardite X", "Blastoisinite", "Mewtwonite X", "Mewtwonite Y", "Blazikenite", "Medichamite", "Houndoominite", "Aggronite", "Banettite", "Tyranitarite",
			"Scizorite", "Pinsirite", "Aerodactylite", "Lucarionite", "Abomasite", "Kangaskhanite", "Gyaradosite", "Absolite", "Charizardite Y", "Alakazite",
			"Heracronite", "Mawilite", "Manectite", "Garchompite", "Latiasite", "Latiosite", "Roseli Berry", "Kee Berry", "Maranga Berry", "Sprinklotad",
			"TM96", "TM97", "TM98", "TM99", "TM100", "Power Plant Pass", "Mega Ring", "Intriguing Stone", "Common Stone", "Discount Coupon",
			"Elevator Key", "TMV Pass", "Honor of Kalos", "Adventure Rules", "Strange Souvenir", "Lens Case", "Makeup Bag", "Travel Trunk", "Lumiose Galette", "Shalour Sable",
			"Jaw Fossil", "Sail Fossil", "Looker Ticket", "Bike", "Holo Caster", "Fairy Gem", "Mega Charm", "Mega Glove", "Mach Bike", "Acro Bike",
			"Wailmer Pail", "Devon Parts", "Soot Sack", "Basement Key", "Pokéblock Kit", "Letter", "Eon Ticket", "Scanner", "Go-Goggles", "Meteorite",
			"Key to Room 1", "Key to Room 2", "Key to Room 4", "Key to Room 6", "Storage Key", "Devon Scope", "S.S. Ticket", "HM07", "Devon Scuba Gear", "Contest Costume",
			"Contest Costume", "Magma Suit", "Aqua Suit", "Pair of Tickets", "Mega Bracelet", "Mega Pendant", "Mega Glasses", "Mega Anchor", "Mega Stickpin", "Mega Tiara",
			"Mega Anklet", "Meteorite", "Swampertite", "Sceptilite", "Sablenite", "Altarianite", "Galladite", "Audinite", "Metagrossite", "Sharpedonite",
			"Slowbronite", "Steelixite", "Pidgeotite", "Glalitite", "Diancite", "Prison Bottle", "Mega Cuff", "Cameruptite", "Lopunnite", "Salamencite",
			"Beedrillite", "Meteorite", "Meteorite", "Key Stone", "Meteorite Shard", "Eon Flute", "n/a", "n/a", "n/a", "Electrium Z",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "Z-Ring", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"Electrium Z", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "UB Ball *", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a"
		});
		this.itembox4.FormattingEnabled = true;
		this.itembox4.Location = new System.Drawing.Point(310, 21);
		this.itembox4.Name = "itembox4";
		this.itembox4.Size = new System.Drawing.Size(107, 21);
		this.itembox4.TabIndex = 7;
		this.itembox3.AllowDrop = true;
		this.itembox3.AutoCompleteCustomSource.AddRange(new string[921]
		{
			"None", "Master Ball", "Ultra Ball", "Great Ball", "Poké Ball", "Safari Ball", "Net Ball", "Dive Ball", "Nest Ball", "Repeat Ball",
			"Timer Ball", "Luxury Ball", "Premier Ball", "Dusk Ball", "Heal Ball", "Quick Ball", "Cherish Ball", "Potion", "Antidote", "Burn Heal",
			"Ice Heal", "Awakening", "Paralyze Heal", "Full Restore", "Max Potion", "Hyper Potion", "Super Potion", "Full Heal", "Revive", "Max Revive",
			"Fresh Water", "Soda Pop", "Lemonade", "Moomoo Milk", "Energy Powder", "Energy Root", "Heal Powder", "Revival Herb", "Ether", "Max Ether",
			"Elixir", "Max Elixir", "Lava Cookie", "Berry Juice", "Sacred Ash", "HP Up", "Protein", "Iron", "Carbos", "Calcium",
			"Rare Candy", "PP Up", "Zinc", "PP Max", "Old Gateau", "Guard Spec.", "Dire Hit", "X Attack", "X Defense", "X Speed",
			"X Accuracy", "X Sp. Atk", "X Sp. Def", "Poké Doll", "Fluffy Tail", "Blue Flute", "Yellow Flute", "Red Flute", "Black Flute", "White Flute",
			"Shoal Salt", "Shoal Shell", "Red Shard", "Blue Shard", "Yellow Shard", "Green Shard", "Super Repel", "Max Repel", "Escape Rope", "Repel",
			"Sun Stone", "Moon Stone", "Fire Stone", "Thunder Stone", "Water Stone", "Leaf Stone", "Tiny Mushroom", "Big Mushroom", "Pearl", "Big Pearl",
			"Stardust", "Star Piece", "Nugget", "Heart Scale", "Honey", "Growth Mulch", "Damp Mulch", "Stable Mulch", "Gooey Mulch", "Root Fossil",
			"Claw Fossil", "Helix Fossil", "Dome Fossil", "Old Amber", "Armor Fossil", "Skull Fossil", "Rare Bone", "Shiny Stone", "Dusk Stone", "Dawn Stone",
			"Oval Stone", "Odd Keystone", "Griseous Orb", "???", "???", "???", "Douse Drive", "Shock Drive", "Burn Drive", "Chill Drive",
			"???", "???", "???", "???", "???", "???", "???", "???", "???", "???",
			"???", "???", "???", "???", "Sweet Heart", "Adamant Orb", "Lustrous Orb", "Greet Mail", "Favored Mail", "RSVP Mail",
			"Thanks Mail", "Inquiry Mail", "Like Mail", "Reply Mail", "Bridge Mail S", "Bridge Mail D", "Bridge Mail T", "Bridge Mail V", "Bridge Mail M", "Cheri Berry",
			"Chesto Berry", "Pecha Berry", "Rawst Berry", "Aspear Berry", "Leppa Berry", "Oran Berry", "Persim Berry", "Lum Berry", "Sitrus Berry", "Figy Berry",
			"Wiki Berry", "Mago Berry", "Aguav Berry", "Iapapa Berry", "Razz Berry", "Bluk Berry", "Nanab Berry", "Wepear Berry", "Pinap Berry", "Pomeg Berry",
			"Kelpsy Berry", "Qualot Berry", "Hondew Berry", "Grepa Berry", "Tamato Berry", "Cornn Berry", "Magost Berry", "Rabuta Berry", "Nomel Berry", "Spelon Berry",
			"Pamtre Berry", "Watmel Berry", "Durin Berry", "Belue Berry", "Occa Berry", "Passho Berry", "Wacan Berry", "Rindo Berry", "Yache Berry", "Chople Berry",
			"Kebia Berry", "Shuca Berry", "Coba Berry", "Payapa Berry", "Tanga Berry", "Charti Berry", "Kasib Berry", "Haban Berry", "Colbur Berry", "Babiri Berry",
			"Chilan Berry", "Liechi Berry", "Ganlon Berry", "Salac Berry", "Petaya Berry", "Apicot Berry", "Lansat Berry", "Starf Berry", "Enigma Berry", "Micle Berry",
			"Custap Berry", "Jaboca Berry", "Rowap Berry", "Bright Powder", "White Herb", "Macho Brace", "Exp. Share", "Quick Claw", "Soothe Bell", "Mental Herb",
			"Choice Band", "King’s Rock", "Silver Powder", "Amulet Coin", "Cleanse Tag", "Soul Dew", "Deep Sea Tooth", "Deep Sea Scale", "Smoke Ball", "Everstone",
			"Focus Band", "Lucky Egg", "Scope Lens", "Metal Coat", "Leftovers", "Dragon Scale", "Light Ball", "Soft Sand", "Hard Stone", "Miracle Seed",
			"Black Glasses", "Black Belt", "Magnet", "Mystic Water", "Sharp Beak", "Poison Barb", "Never-Melt Ice", "Spell Tag", "Twisted Spoon", "Charcoal",
			"Dragon Fang", "Silk Scarf", "Up-Grade", "Shell Bell", "Sea Incense", "Lax Incense", "Lucky Punch", "Metal Powder", "Thick Club", "Stick",
			"Red Scarf", "Blue Scarf", "Pink Scarf", "Green Scarf", "Yellow Scarf", "Wide Lens", "Muscle Band", "Wise Glasses", "Expert Belt", "Light Clay",
			"Life Orb", "Power Herb", "Toxic Orb", "Flame Orb", "Quick Powder", "Focus Sash", "Zoom Lens", "Metronome", "Iron Ball", "Lagging Tail",
			"Destiny Knot", "Black Sludge", "Icy Rock", "Smooth Rock", "Heat Rock", "Damp Rock", "Grip Claw", "Choice Scarf", "Sticky Barb", "Power Bracer",
			"Power Belt", "Power Lens", "Power Band", "Power Anklet", "Power Weight", "Shed Shell", "Big Root", "Choice Specs", "Flame Plate", "Splash Plate",
			"Zap Plate", "Meadow Plate", "Icicle Plate", "Fist Plate", "Toxic Plate", "Earth Plate", "Sky Plate", "Mind Plate", "Insect Plate", "Stone Plate",
			"Spooky Plate", "Draco Plate", "Dread Plate", "Iron Plate", "Odd Incense", "Rock Incense", "Full Incense", "Wave Incense", "Rose Incense", "Luck Incense",
			"Pure Incense", "Protector", "Electirizer", "Magmarizer", "Dubious Disc", "Reaper Cloth", "Razor Claw", "Razor Fang", "TM01", "TM02",
			"TM03", "TM04", "TM05", "TM06", "TM07", "TM08", "TM09", "TM10", "TM11", "TM12",
			"TM13", "TM14", "TM15", "TM16", "TM17", "TM18", "TM19", "TM20", "TM21", "TM22",
			"TM23", "TM24", "TM25", "TM26", "TM27", "TM28", "TM29", "TM30", "TM31", "TM32",
			"TM33", "TM34", "TM35", "TM36", "TM37", "TM38", "TM39", "TM40", "TM41", "TM42",
			"TM43", "TM44", "TM45", "TM46", "TM47", "TM48", "TM49", "TM50", "TM51", "TM52",
			"TM53", "TM54", "TM55", "TM56", "TM57", "TM58", "TM59", "TM60", "TM61", "TM62",
			"TM63", "TM64", "TM65", "TM66", "TM67", "TM68", "TM69", "TM70", "TM71", "TM72",
			"TM73", "TM74", "TM75", "TM76", "TM77", "TM78", "TM79", "TM80", "TM81", "TM82",
			"TM83", "TM84", "TM85", "TM86", "TM87", "TM88", "TM89", "TM90", "TM91", "TM92",
			"HM01", "HM02", "HM03", "HM04", "HM05", "HM06", "???", "???", "Explorer Kit", "Loot Sack",
			"Rule Book", "Poké Radar", "Point Card", "Journal", "Seal Case", "Fashion Case", "Seal Bag", "Pal Pad", "Works Key", "Old Charm",
			"Galactic Key", "Red Chain", "Town Map", "Vs. Seeker", "Coin Case", "Old Rod", "Good Rod", "Super Rod", "Sprayduck", "Poffin Case",
			"Bike", "Suite Key", "Oak’s Letter", "Lunar Wing", "Member Card", "Azure Flute", "S.S. Ticket", "Contest Pass", "Magma Stone", "Parcel",
			"Coupon 1", "Coupon 2", "Coupon 3", "Storage Key", "Secret Potion", "Vs. Recorder", "Gracidea", "Secret Key", "Apricorn Box", "Unown Report",
			"Berry Pots", "Dowsing Machine", "Blue Card", "Slowpoke Tail", "Clear Bell", "Card Key", "Basement Key", "Squirt Bottle", "Red Scale", "Lost Item",
			"Pass", "Machine Part", "Silver Wing", "Rainbow Wing", "Mystery Egg", "Red Apricorn", "Blue Apricorn", "Yellow Apricorn", "Green Apricorn", "Pink Apricorn",
			"White Apricorn", "Black Apricorn", "Fast Ball", "Level Ball", "Lure Ball", "Heavy Ball", "Love Ball", "Friend Ball", "Moon Ball", "Sport Ball",
			"Park Ball", "Photo Album", "GB Sounds", "Tidal Bell", "Rage Candy Bar", "Data Card 01", "Data Card 02", "Data Card 03", "Data Card 04", "Data Card 05",
			"Data Card 06", "Data Card 07", "Data Card 08", "Data Card 09", "Data Card 10", "Data Card 11", "Data Card 12", "Data Card 13", "Data Card 14", "Data Card 15",
			"Data Card 16", "Data Card 17", "Data Card 18", "Data Card 19", "Data Card 20", "Data Card 21", "Data Card 22", "Data Card 23", "Data Card 24", "Data Card 25",
			"Data Card 26", "Data Card 27", "Jade Orb", "Lock Capsule", "Red Orb", "Blue Orb", "Enigma Stone", "Prism Scale", "Eviolite", "Float Stone",
			"Rocky Helmet", "Air Balloon", "Red Card", "Ring Target", "Binding Band", "Absorb Bulb", "Cell Battery", "Eject Button", "Fire Gem", "Water Gem",
			"Electric Gem", "Grass Gem", "Ice Gem", "Fighting Gem", "Poison Gem", "Ground Gem", "Flying Gem", "Psychic Gem", "Bug Gem", "Rock Gem",
			"Ghost Gem", "Dragon Gem", "Dark Gem", "Steel Gem", "Normal Gem", "Health Wing", "Muscle Wing", "Resist Wing", "Genius Wing", "Clever Wing",
			"Swift Wing", "Pretty Wing", "Cover Fossil", "Plume Fossil", "Liberty Pass", "Pass Orb", "Dream Ball", "Poké Toy", "Prop Case", "Dragon Skull",
			"Balm Mushroom", "Big Nugget", "Pearl String", "Comet Shard", "Relic Copper", "Relic Silver", "Relic Gold", "Relic Vase", "Relic Band", "Relic Statue",
			"Relic Crown", "Casteliacone", "Dire Hit 2", "X Speed 2", "X Sp. Atk 2", "X Sp. Def 2", "X Defense 2", "X Attack 2", "X Accuracy 2", "X Speed 3",
			"X Sp. Atk 3", "X Sp. Def 3", "X Defense 3", "X Attack 3", "X Accuracy 3", "X Speed 6", "X Sp. Atk 6", "X Sp. Def 6", "X Defense 6", "X Attack 6",
			"X Accuracy 6", "Ability Urge", "Item Drop", "Item Urge", "Reset Urge", "Dire Hit 3", "Light Stone", "Dark Stone", "TM93", "TM94",
			"TM95", "Xtransceiver", "???", "Gram 1", "Gram 2", "Gram 3", "Xtransceiver", "Medal Box", "DNA Splicers", "DNA Splicers",
			"Permit", "Oval Charm", "Shiny Charm", "Plasma Card", "Grubby Hanky", "Colress Machine", "Dropped Item", "Dropped Item", "Reveal Glass", "Weakness Policy",
			"Assault Vest", "Holo Caster", "Prof’s Letter", "Roller Skates", "Pixie Plate", "Ability Capsule", "Whipped Dream", "Sachet", "Luminous Moss", "Snowball",
			"Safety Goggles", "Poké Flute", "Rich Mulch", "Surprise Mulch", "Boost Mulch", "Amaze Mulch", "Gengarite", "Gardevoirite", "Ampharosite", "Venusaurite",
			"Charizardite X", "Blastoisinite", "Mewtwonite X", "Mewtwonite Y", "Blazikenite", "Medichamite", "Houndoominite", "Aggronite", "Banettite", "Tyranitarite",
			"Scizorite", "Pinsirite", "Aerodactylite", "Lucarionite", "Abomasite", "Kangaskhanite", "Gyaradosite", "Absolite", "Charizardite Y", "Alakazite",
			"Heracronite", "Mawilite", "Manectite", "Garchompite", "Latiasite", "Latiosite", "Roseli Berry", "Kee Berry", "Maranga Berry", "Sprinklotad",
			"TM96", "TM97", "TM98", "TM99", "TM100", "Power Plant Pass", "Mega Ring", "Intriguing Stone", "Common Stone", "Discount Coupon",
			"Elevator Key", "TMV Pass", "Honor of Kalos", "Adventure Rules", "Strange Souvenir", "Lens Case", "Makeup Bag", "Travel Trunk", "Lumiose Galette", "Shalour Sable",
			"Jaw Fossil", "Sail Fossil", "Looker Ticket", "Bike", "Holo Caster", "Fairy Gem", "Mega Charm", "Mega Glove", "Mach Bike", "Acro Bike",
			"Wailmer Pail", "Devon Parts", "Soot Sack", "Basement Key", "Pokéblock Kit", "Letter", "Eon Ticket", "Scanner", "Go-Goggles", "Meteorite",
			"Key to Room 1", "Key to Room 2", "Key to Room 4", "Key to Room 6", "Storage Key", "Devon Scope", "S.S. Ticket", "HM07", "Devon Scuba Gear", "Contest Costume",
			"Contest Costume", "Magma Suit", "Aqua Suit", "Pair of Tickets", "Mega Bracelet", "Mega Pendant", "Mega Glasses", "Mega Anchor", "Mega Stickpin", "Mega Tiara",
			"Mega Anklet", "Meteorite", "Swampertite", "Sceptilite", "Sablenite", "Altarianite", "Galladite", "Audinite", "Metagrossite", "Sharpedonite",
			"Slowbronite", "Steelixite", "Pidgeotite", "Glalitite", "Diancite", "Prison Bottle", "Mega Cuff", "Cameruptite", "Lopunnite", "Salamencite",
			"Beedrillite", "Meteorite", "Meteorite", "Key Stone", "Meteorite Shard", "Eon Flute", "n/a", "n/a", "n/a", "Electrium Z",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "Z-Ring", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"Electrium Z", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "UB Ball *", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a"
		});
		this.itembox3.FormattingEnabled = true;
		this.itembox3.Location = new System.Drawing.Point(78, 75);
		this.itembox3.Name = "itembox3";
		this.itembox3.Size = new System.Drawing.Size(107, 21);
		this.itembox3.TabIndex = 5;
		this.itembox2.AllowDrop = true;
		this.itembox2.AutoCompleteCustomSource.AddRange(new string[921]
		{
			"None", "Master Ball", "Ultra Ball", "Great Ball", "Poké Ball", "Safari Ball", "Net Ball", "Dive Ball", "Nest Ball", "Repeat Ball",
			"Timer Ball", "Luxury Ball", "Premier Ball", "Dusk Ball", "Heal Ball", "Quick Ball", "Cherish Ball", "Potion", "Antidote", "Burn Heal",
			"Ice Heal", "Awakening", "Paralyze Heal", "Full Restore", "Max Potion", "Hyper Potion", "Super Potion", "Full Heal", "Revive", "Max Revive",
			"Fresh Water", "Soda Pop", "Lemonade", "Moomoo Milk", "Energy Powder", "Energy Root", "Heal Powder", "Revival Herb", "Ether", "Max Ether",
			"Elixir", "Max Elixir", "Lava Cookie", "Berry Juice", "Sacred Ash", "HP Up", "Protein", "Iron", "Carbos", "Calcium",
			"Rare Candy", "PP Up", "Zinc", "PP Max", "Old Gateau", "Guard Spec.", "Dire Hit", "X Attack", "X Defense", "X Speed",
			"X Accuracy", "X Sp. Atk", "X Sp. Def", "Poké Doll", "Fluffy Tail", "Blue Flute", "Yellow Flute", "Red Flute", "Black Flute", "White Flute",
			"Shoal Salt", "Shoal Shell", "Red Shard", "Blue Shard", "Yellow Shard", "Green Shard", "Super Repel", "Max Repel", "Escape Rope", "Repel",
			"Sun Stone", "Moon Stone", "Fire Stone", "Thunder Stone", "Water Stone", "Leaf Stone", "Tiny Mushroom", "Big Mushroom", "Pearl", "Big Pearl",
			"Stardust", "Star Piece", "Nugget", "Heart Scale", "Honey", "Growth Mulch", "Damp Mulch", "Stable Mulch", "Gooey Mulch", "Root Fossil",
			"Claw Fossil", "Helix Fossil", "Dome Fossil", "Old Amber", "Armor Fossil", "Skull Fossil", "Rare Bone", "Shiny Stone", "Dusk Stone", "Dawn Stone",
			"Oval Stone", "Odd Keystone", "Griseous Orb", "???", "???", "???", "Douse Drive", "Shock Drive", "Burn Drive", "Chill Drive",
			"???", "???", "???", "???", "???", "???", "???", "???", "???", "???",
			"???", "???", "???", "???", "Sweet Heart", "Adamant Orb", "Lustrous Orb", "Greet Mail", "Favored Mail", "RSVP Mail",
			"Thanks Mail", "Inquiry Mail", "Like Mail", "Reply Mail", "Bridge Mail S", "Bridge Mail D", "Bridge Mail T", "Bridge Mail V", "Bridge Mail M", "Cheri Berry",
			"Chesto Berry", "Pecha Berry", "Rawst Berry", "Aspear Berry", "Leppa Berry", "Oran Berry", "Persim Berry", "Lum Berry", "Sitrus Berry", "Figy Berry",
			"Wiki Berry", "Mago Berry", "Aguav Berry", "Iapapa Berry", "Razz Berry", "Bluk Berry", "Nanab Berry", "Wepear Berry", "Pinap Berry", "Pomeg Berry",
			"Kelpsy Berry", "Qualot Berry", "Hondew Berry", "Grepa Berry", "Tamato Berry", "Cornn Berry", "Magost Berry", "Rabuta Berry", "Nomel Berry", "Spelon Berry",
			"Pamtre Berry", "Watmel Berry", "Durin Berry", "Belue Berry", "Occa Berry", "Passho Berry", "Wacan Berry", "Rindo Berry", "Yache Berry", "Chople Berry",
			"Kebia Berry", "Shuca Berry", "Coba Berry", "Payapa Berry", "Tanga Berry", "Charti Berry", "Kasib Berry", "Haban Berry", "Colbur Berry", "Babiri Berry",
			"Chilan Berry", "Liechi Berry", "Ganlon Berry", "Salac Berry", "Petaya Berry", "Apicot Berry", "Lansat Berry", "Starf Berry", "Enigma Berry", "Micle Berry",
			"Custap Berry", "Jaboca Berry", "Rowap Berry", "Bright Powder", "White Herb", "Macho Brace", "Exp. Share", "Quick Claw", "Soothe Bell", "Mental Herb",
			"Choice Band", "King’s Rock", "Silver Powder", "Amulet Coin", "Cleanse Tag", "Soul Dew", "Deep Sea Tooth", "Deep Sea Scale", "Smoke Ball", "Everstone",
			"Focus Band", "Lucky Egg", "Scope Lens", "Metal Coat", "Leftovers", "Dragon Scale", "Light Ball", "Soft Sand", "Hard Stone", "Miracle Seed",
			"Black Glasses", "Black Belt", "Magnet", "Mystic Water", "Sharp Beak", "Poison Barb", "Never-Melt Ice", "Spell Tag", "Twisted Spoon", "Charcoal",
			"Dragon Fang", "Silk Scarf", "Up-Grade", "Shell Bell", "Sea Incense", "Lax Incense", "Lucky Punch", "Metal Powder", "Thick Club", "Stick",
			"Red Scarf", "Blue Scarf", "Pink Scarf", "Green Scarf", "Yellow Scarf", "Wide Lens", "Muscle Band", "Wise Glasses", "Expert Belt", "Light Clay",
			"Life Orb", "Power Herb", "Toxic Orb", "Flame Orb", "Quick Powder", "Focus Sash", "Zoom Lens", "Metronome", "Iron Ball", "Lagging Tail",
			"Destiny Knot", "Black Sludge", "Icy Rock", "Smooth Rock", "Heat Rock", "Damp Rock", "Grip Claw", "Choice Scarf", "Sticky Barb", "Power Bracer",
			"Power Belt", "Power Lens", "Power Band", "Power Anklet", "Power Weight", "Shed Shell", "Big Root", "Choice Specs", "Flame Plate", "Splash Plate",
			"Zap Plate", "Meadow Plate", "Icicle Plate", "Fist Plate", "Toxic Plate", "Earth Plate", "Sky Plate", "Mind Plate", "Insect Plate", "Stone Plate",
			"Spooky Plate", "Draco Plate", "Dread Plate", "Iron Plate", "Odd Incense", "Rock Incense", "Full Incense", "Wave Incense", "Rose Incense", "Luck Incense",
			"Pure Incense", "Protector", "Electirizer", "Magmarizer", "Dubious Disc", "Reaper Cloth", "Razor Claw", "Razor Fang", "TM01", "TM02",
			"TM03", "TM04", "TM05", "TM06", "TM07", "TM08", "TM09", "TM10", "TM11", "TM12",
			"TM13", "TM14", "TM15", "TM16", "TM17", "TM18", "TM19", "TM20", "TM21", "TM22",
			"TM23", "TM24", "TM25", "TM26", "TM27", "TM28", "TM29", "TM30", "TM31", "TM32",
			"TM33", "TM34", "TM35", "TM36", "TM37", "TM38", "TM39", "TM40", "TM41", "TM42",
			"TM43", "TM44", "TM45", "TM46", "TM47", "TM48", "TM49", "TM50", "TM51", "TM52",
			"TM53", "TM54", "TM55", "TM56", "TM57", "TM58", "TM59", "TM60", "TM61", "TM62",
			"TM63", "TM64", "TM65", "TM66", "TM67", "TM68", "TM69", "TM70", "TM71", "TM72",
			"TM73", "TM74", "TM75", "TM76", "TM77", "TM78", "TM79", "TM80", "TM81", "TM82",
			"TM83", "TM84", "TM85", "TM86", "TM87", "TM88", "TM89", "TM90", "TM91", "TM92",
			"HM01", "HM02", "HM03", "HM04", "HM05", "HM06", "???", "???", "Explorer Kit", "Loot Sack",
			"Rule Book", "Poké Radar", "Point Card", "Journal", "Seal Case", "Fashion Case", "Seal Bag", "Pal Pad", "Works Key", "Old Charm",
			"Galactic Key", "Red Chain", "Town Map", "Vs. Seeker", "Coin Case", "Old Rod", "Good Rod", "Super Rod", "Sprayduck", "Poffin Case",
			"Bike", "Suite Key", "Oak’s Letter", "Lunar Wing", "Member Card", "Azure Flute", "S.S. Ticket", "Contest Pass", "Magma Stone", "Parcel",
			"Coupon 1", "Coupon 2", "Coupon 3", "Storage Key", "Secret Potion", "Vs. Recorder", "Gracidea", "Secret Key", "Apricorn Box", "Unown Report",
			"Berry Pots", "Dowsing Machine", "Blue Card", "Slowpoke Tail", "Clear Bell", "Card Key", "Basement Key", "Squirt Bottle", "Red Scale", "Lost Item",
			"Pass", "Machine Part", "Silver Wing", "Rainbow Wing", "Mystery Egg", "Red Apricorn", "Blue Apricorn", "Yellow Apricorn", "Green Apricorn", "Pink Apricorn",
			"White Apricorn", "Black Apricorn", "Fast Ball", "Level Ball", "Lure Ball", "Heavy Ball", "Love Ball", "Friend Ball", "Moon Ball", "Sport Ball",
			"Park Ball", "Photo Album", "GB Sounds", "Tidal Bell", "Rage Candy Bar", "Data Card 01", "Data Card 02", "Data Card 03", "Data Card 04", "Data Card 05",
			"Data Card 06", "Data Card 07", "Data Card 08", "Data Card 09", "Data Card 10", "Data Card 11", "Data Card 12", "Data Card 13", "Data Card 14", "Data Card 15",
			"Data Card 16", "Data Card 17", "Data Card 18", "Data Card 19", "Data Card 20", "Data Card 21", "Data Card 22", "Data Card 23", "Data Card 24", "Data Card 25",
			"Data Card 26", "Data Card 27", "Jade Orb", "Lock Capsule", "Red Orb", "Blue Orb", "Enigma Stone", "Prism Scale", "Eviolite", "Float Stone",
			"Rocky Helmet", "Air Balloon", "Red Card", "Ring Target", "Binding Band", "Absorb Bulb", "Cell Battery", "Eject Button", "Fire Gem", "Water Gem",
			"Electric Gem", "Grass Gem", "Ice Gem", "Fighting Gem", "Poison Gem", "Ground Gem", "Flying Gem", "Psychic Gem", "Bug Gem", "Rock Gem",
			"Ghost Gem", "Dragon Gem", "Dark Gem", "Steel Gem", "Normal Gem", "Health Wing", "Muscle Wing", "Resist Wing", "Genius Wing", "Clever Wing",
			"Swift Wing", "Pretty Wing", "Cover Fossil", "Plume Fossil", "Liberty Pass", "Pass Orb", "Dream Ball", "Poké Toy", "Prop Case", "Dragon Skull",
			"Balm Mushroom", "Big Nugget", "Pearl String", "Comet Shard", "Relic Copper", "Relic Silver", "Relic Gold", "Relic Vase", "Relic Band", "Relic Statue",
			"Relic Crown", "Casteliacone", "Dire Hit 2", "X Speed 2", "X Sp. Atk 2", "X Sp. Def 2", "X Defense 2", "X Attack 2", "X Accuracy 2", "X Speed 3",
			"X Sp. Atk 3", "X Sp. Def 3", "X Defense 3", "X Attack 3", "X Accuracy 3", "X Speed 6", "X Sp. Atk 6", "X Sp. Def 6", "X Defense 6", "X Attack 6",
			"X Accuracy 6", "Ability Urge", "Item Drop", "Item Urge", "Reset Urge", "Dire Hit 3", "Light Stone", "Dark Stone", "TM93", "TM94",
			"TM95", "Xtransceiver", "???", "Gram 1", "Gram 2", "Gram 3", "Xtransceiver", "Medal Box", "DNA Splicers", "DNA Splicers",
			"Permit", "Oval Charm", "Shiny Charm", "Plasma Card", "Grubby Hanky", "Colress Machine", "Dropped Item", "Dropped Item", "Reveal Glass", "Weakness Policy",
			"Assault Vest", "Holo Caster", "Prof’s Letter", "Roller Skates", "Pixie Plate", "Ability Capsule", "Whipped Dream", "Sachet", "Luminous Moss", "Snowball",
			"Safety Goggles", "Poké Flute", "Rich Mulch", "Surprise Mulch", "Boost Mulch", "Amaze Mulch", "Gengarite", "Gardevoirite", "Ampharosite", "Venusaurite",
			"Charizardite X", "Blastoisinite", "Mewtwonite X", "Mewtwonite Y", "Blazikenite", "Medichamite", "Houndoominite", "Aggronite", "Banettite", "Tyranitarite",
			"Scizorite", "Pinsirite", "Aerodactylite", "Lucarionite", "Abomasite", "Kangaskhanite", "Gyaradosite", "Absolite", "Charizardite Y", "Alakazite",
			"Heracronite", "Mawilite", "Manectite", "Garchompite", "Latiasite", "Latiosite", "Roseli Berry", "Kee Berry", "Maranga Berry", "Sprinklotad",
			"TM96", "TM97", "TM98", "TM99", "TM100", "Power Plant Pass", "Mega Ring", "Intriguing Stone", "Common Stone", "Discount Coupon",
			"Elevator Key", "TMV Pass", "Honor of Kalos", "Adventure Rules", "Strange Souvenir", "Lens Case", "Makeup Bag", "Travel Trunk", "Lumiose Galette", "Shalour Sable",
			"Jaw Fossil", "Sail Fossil", "Looker Ticket", "Bike", "Holo Caster", "Fairy Gem", "Mega Charm", "Mega Glove", "Mach Bike", "Acro Bike",
			"Wailmer Pail", "Devon Parts", "Soot Sack", "Basement Key", "Pokéblock Kit", "Letter", "Eon Ticket", "Scanner", "Go-Goggles", "Meteorite",
			"Key to Room 1", "Key to Room 2", "Key to Room 4", "Key to Room 6", "Storage Key", "Devon Scope", "S.S. Ticket", "HM07", "Devon Scuba Gear", "Contest Costume",
			"Contest Costume", "Magma Suit", "Aqua Suit", "Pair of Tickets", "Mega Bracelet", "Mega Pendant", "Mega Glasses", "Mega Anchor", "Mega Stickpin", "Mega Tiara",
			"Mega Anklet", "Meteorite", "Swampertite", "Sceptilite", "Sablenite", "Altarianite", "Galladite", "Audinite", "Metagrossite", "Sharpedonite",
			"Slowbronite", "Steelixite", "Pidgeotite", "Glalitite", "Diancite", "Prison Bottle", "Mega Cuff", "Cameruptite", "Lopunnite", "Salamencite",
			"Beedrillite", "Meteorite", "Meteorite", "Key Stone", "Meteorite Shard", "Eon Flute", "n/a", "n/a", "n/a", "Electrium Z",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "Z-Ring", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"Electrium Z", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "UB Ball *", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a"
		});
		this.itembox2.FormattingEnabled = true;
		this.itembox2.Location = new System.Drawing.Point(78, 48);
		this.itembox2.Name = "itembox2";
		this.itembox2.Size = new System.Drawing.Size(107, 21);
		this.itembox2.TabIndex = 3;
		this.itembox1.AllowDrop = true;
		this.itembox1.AutoCompleteCustomSource.AddRange(new string[921]
		{
			"None", "Master Ball", "Ultra Ball", "Great Ball", "Poké Ball", "Safari Ball", "Net Ball", "Dive Ball", "Nest Ball", "Repeat Ball",
			"Timer Ball", "Luxury Ball", "Premier Ball", "Dusk Ball", "Heal Ball", "Quick Ball", "Cherish Ball", "Potion", "Antidote", "Burn Heal",
			"Ice Heal", "Awakening", "Paralyze Heal", "Full Restore", "Max Potion", "Hyper Potion", "Super Potion", "Full Heal", "Revive", "Max Revive",
			"Fresh Water", "Soda Pop", "Lemonade", "Moomoo Milk", "Energy Powder", "Energy Root", "Heal Powder", "Revival Herb", "Ether", "Max Ether",
			"Elixir", "Max Elixir", "Lava Cookie", "Berry Juice", "Sacred Ash", "HP Up", "Protein", "Iron", "Carbos", "Calcium",
			"Rare Candy", "PP Up", "Zinc", "PP Max", "Old Gateau", "Guard Spec.", "Dire Hit", "X Attack", "X Defense", "X Speed",
			"X Accuracy", "X Sp. Atk", "X Sp. Def", "Poké Doll", "Fluffy Tail", "Blue Flute", "Yellow Flute", "Red Flute", "Black Flute", "White Flute",
			"Shoal Salt", "Shoal Shell", "Red Shard", "Blue Shard", "Yellow Shard", "Green Shard", "Super Repel", "Max Repel", "Escape Rope", "Repel",
			"Sun Stone", "Moon Stone", "Fire Stone", "Thunder Stone", "Water Stone", "Leaf Stone", "Tiny Mushroom", "Big Mushroom", "Pearl", "Big Pearl",
			"Stardust", "Star Piece", "Nugget", "Heart Scale", "Honey", "Growth Mulch", "Damp Mulch", "Stable Mulch", "Gooey Mulch", "Root Fossil",
			"Claw Fossil", "Helix Fossil", "Dome Fossil", "Old Amber", "Armor Fossil", "Skull Fossil", "Rare Bone", "Shiny Stone", "Dusk Stone", "Dawn Stone",
			"Oval Stone", "Odd Keystone", "Griseous Orb", "???", "???", "???", "Douse Drive", "Shock Drive", "Burn Drive", "Chill Drive",
			"???", "???", "???", "???", "???", "???", "???", "???", "???", "???",
			"???", "???", "???", "???", "Sweet Heart", "Adamant Orb", "Lustrous Orb", "Greet Mail", "Favored Mail", "RSVP Mail",
			"Thanks Mail", "Inquiry Mail", "Like Mail", "Reply Mail", "Bridge Mail S", "Bridge Mail D", "Bridge Mail T", "Bridge Mail V", "Bridge Mail M", "Cheri Berry",
			"Chesto Berry", "Pecha Berry", "Rawst Berry", "Aspear Berry", "Leppa Berry", "Oran Berry", "Persim Berry", "Lum Berry", "Sitrus Berry", "Figy Berry",
			"Wiki Berry", "Mago Berry", "Aguav Berry", "Iapapa Berry", "Razz Berry", "Bluk Berry", "Nanab Berry", "Wepear Berry", "Pinap Berry", "Pomeg Berry",
			"Kelpsy Berry", "Qualot Berry", "Hondew Berry", "Grepa Berry", "Tamato Berry", "Cornn Berry", "Magost Berry", "Rabuta Berry", "Nomel Berry", "Spelon Berry",
			"Pamtre Berry", "Watmel Berry", "Durin Berry", "Belue Berry", "Occa Berry", "Passho Berry", "Wacan Berry", "Rindo Berry", "Yache Berry", "Chople Berry",
			"Kebia Berry", "Shuca Berry", "Coba Berry", "Payapa Berry", "Tanga Berry", "Charti Berry", "Kasib Berry", "Haban Berry", "Colbur Berry", "Babiri Berry",
			"Chilan Berry", "Liechi Berry", "Ganlon Berry", "Salac Berry", "Petaya Berry", "Apicot Berry", "Lansat Berry", "Starf Berry", "Enigma Berry", "Micle Berry",
			"Custap Berry", "Jaboca Berry", "Rowap Berry", "Bright Powder", "White Herb", "Macho Brace", "Exp. Share", "Quick Claw", "Soothe Bell", "Mental Herb",
			"Choice Band", "King’s Rock", "Silver Powder", "Amulet Coin", "Cleanse Tag", "Soul Dew", "Deep Sea Tooth", "Deep Sea Scale", "Smoke Ball", "Everstone",
			"Focus Band", "Lucky Egg", "Scope Lens", "Metal Coat", "Leftovers", "Dragon Scale", "Light Ball", "Soft Sand", "Hard Stone", "Miracle Seed",
			"Black Glasses", "Black Belt", "Magnet", "Mystic Water", "Sharp Beak", "Poison Barb", "Never-Melt Ice", "Spell Tag", "Twisted Spoon", "Charcoal",
			"Dragon Fang", "Silk Scarf", "Up-Grade", "Shell Bell", "Sea Incense", "Lax Incense", "Lucky Punch", "Metal Powder", "Thick Club", "Stick",
			"Red Scarf", "Blue Scarf", "Pink Scarf", "Green Scarf", "Yellow Scarf", "Wide Lens", "Muscle Band", "Wise Glasses", "Expert Belt", "Light Clay",
			"Life Orb", "Power Herb", "Toxic Orb", "Flame Orb", "Quick Powder", "Focus Sash", "Zoom Lens", "Metronome", "Iron Ball", "Lagging Tail",
			"Destiny Knot", "Black Sludge", "Icy Rock", "Smooth Rock", "Heat Rock", "Damp Rock", "Grip Claw", "Choice Scarf", "Sticky Barb", "Power Bracer",
			"Power Belt", "Power Lens", "Power Band", "Power Anklet", "Power Weight", "Shed Shell", "Big Root", "Choice Specs", "Flame Plate", "Splash Plate",
			"Zap Plate", "Meadow Plate", "Icicle Plate", "Fist Plate", "Toxic Plate", "Earth Plate", "Sky Plate", "Mind Plate", "Insect Plate", "Stone Plate",
			"Spooky Plate", "Draco Plate", "Dread Plate", "Iron Plate", "Odd Incense", "Rock Incense", "Full Incense", "Wave Incense", "Rose Incense", "Luck Incense",
			"Pure Incense", "Protector", "Electirizer", "Magmarizer", "Dubious Disc", "Reaper Cloth", "Razor Claw", "Razor Fang", "TM01", "TM02",
			"TM03", "TM04", "TM05", "TM06", "TM07", "TM08", "TM09", "TM10", "TM11", "TM12",
			"TM13", "TM14", "TM15", "TM16", "TM17", "TM18", "TM19", "TM20", "TM21", "TM22",
			"TM23", "TM24", "TM25", "TM26", "TM27", "TM28", "TM29", "TM30", "TM31", "TM32",
			"TM33", "TM34", "TM35", "TM36", "TM37", "TM38", "TM39", "TM40", "TM41", "TM42",
			"TM43", "TM44", "TM45", "TM46", "TM47", "TM48", "TM49", "TM50", "TM51", "TM52",
			"TM53", "TM54", "TM55", "TM56", "TM57", "TM58", "TM59", "TM60", "TM61", "TM62",
			"TM63", "TM64", "TM65", "TM66", "TM67", "TM68", "TM69", "TM70", "TM71", "TM72",
			"TM73", "TM74", "TM75", "TM76", "TM77", "TM78", "TM79", "TM80", "TM81", "TM82",
			"TM83", "TM84", "TM85", "TM86", "TM87", "TM88", "TM89", "TM90", "TM91", "TM92",
			"HM01", "HM02", "HM03", "HM04", "HM05", "HM06", "???", "???", "Explorer Kit", "Loot Sack",
			"Rule Book", "Poké Radar", "Point Card", "Journal", "Seal Case", "Fashion Case", "Seal Bag", "Pal Pad", "Works Key", "Old Charm",
			"Galactic Key", "Red Chain", "Town Map", "Vs. Seeker", "Coin Case", "Old Rod", "Good Rod", "Super Rod", "Sprayduck", "Poffin Case",
			"Bike", "Suite Key", "Oak’s Letter", "Lunar Wing", "Member Card", "Azure Flute", "S.S. Ticket", "Contest Pass", "Magma Stone", "Parcel",
			"Coupon 1", "Coupon 2", "Coupon 3", "Storage Key", "Secret Potion", "Vs. Recorder", "Gracidea", "Secret Key", "Apricorn Box", "Unown Report",
			"Berry Pots", "Dowsing Machine", "Blue Card", "Slowpoke Tail", "Clear Bell", "Card Key", "Basement Key", "Squirt Bottle", "Red Scale", "Lost Item",
			"Pass", "Machine Part", "Silver Wing", "Rainbow Wing", "Mystery Egg", "Red Apricorn", "Blue Apricorn", "Yellow Apricorn", "Green Apricorn", "Pink Apricorn",
			"White Apricorn", "Black Apricorn", "Fast Ball", "Level Ball", "Lure Ball", "Heavy Ball", "Love Ball", "Friend Ball", "Moon Ball", "Sport Ball",
			"Park Ball", "Photo Album", "GB Sounds", "Tidal Bell", "Rage Candy Bar", "Data Card 01", "Data Card 02", "Data Card 03", "Data Card 04", "Data Card 05",
			"Data Card 06", "Data Card 07", "Data Card 08", "Data Card 09", "Data Card 10", "Data Card 11", "Data Card 12", "Data Card 13", "Data Card 14", "Data Card 15",
			"Data Card 16", "Data Card 17", "Data Card 18", "Data Card 19", "Data Card 20", "Data Card 21", "Data Card 22", "Data Card 23", "Data Card 24", "Data Card 25",
			"Data Card 26", "Data Card 27", "Jade Orb", "Lock Capsule", "Red Orb", "Blue Orb", "Enigma Stone", "Prism Scale", "Eviolite", "Float Stone",
			"Rocky Helmet", "Air Balloon", "Red Card", "Ring Target", "Binding Band", "Absorb Bulb", "Cell Battery", "Eject Button", "Fire Gem", "Water Gem",
			"Electric Gem", "Grass Gem", "Ice Gem", "Fighting Gem", "Poison Gem", "Ground Gem", "Flying Gem", "Psychic Gem", "Bug Gem", "Rock Gem",
			"Ghost Gem", "Dragon Gem", "Dark Gem", "Steel Gem", "Normal Gem", "Health Wing", "Muscle Wing", "Resist Wing", "Genius Wing", "Clever Wing",
			"Swift Wing", "Pretty Wing", "Cover Fossil", "Plume Fossil", "Liberty Pass", "Pass Orb", "Dream Ball", "Poké Toy", "Prop Case", "Dragon Skull",
			"Balm Mushroom", "Big Nugget", "Pearl String", "Comet Shard", "Relic Copper", "Relic Silver", "Relic Gold", "Relic Vase", "Relic Band", "Relic Statue",
			"Relic Crown", "Casteliacone", "Dire Hit 2", "X Speed 2", "X Sp. Atk 2", "X Sp. Def 2", "X Defense 2", "X Attack 2", "X Accuracy 2", "X Speed 3",
			"X Sp. Atk 3", "X Sp. Def 3", "X Defense 3", "X Attack 3", "X Accuracy 3", "X Speed 6", "X Sp. Atk 6", "X Sp. Def 6", "X Defense 6", "X Attack 6",
			"X Accuracy 6", "Ability Urge", "Item Drop", "Item Urge", "Reset Urge", "Dire Hit 3", "Light Stone", "Dark Stone", "TM93", "TM94",
			"TM95", "Xtransceiver", "???", "Gram 1", "Gram 2", "Gram 3", "Xtransceiver", "Medal Box", "DNA Splicers", "DNA Splicers",
			"Permit", "Oval Charm", "Shiny Charm", "Plasma Card", "Grubby Hanky", "Colress Machine", "Dropped Item", "Dropped Item", "Reveal Glass", "Weakness Policy",
			"Assault Vest", "Holo Caster", "Prof’s Letter", "Roller Skates", "Pixie Plate", "Ability Capsule", "Whipped Dream", "Sachet", "Luminous Moss", "Snowball",
			"Safety Goggles", "Poké Flute", "Rich Mulch", "Surprise Mulch", "Boost Mulch", "Amaze Mulch", "Gengarite", "Gardevoirite", "Ampharosite", "Venusaurite",
			"Charizardite X", "Blastoisinite", "Mewtwonite X", "Mewtwonite Y", "Blazikenite", "Medichamite", "Houndoominite", "Aggronite", "Banettite", "Tyranitarite",
			"Scizorite", "Pinsirite", "Aerodactylite", "Lucarionite", "Abomasite", "Kangaskhanite", "Gyaradosite", "Absolite", "Charizardite Y", "Alakazite",
			"Heracronite", "Mawilite", "Manectite", "Garchompite", "Latiasite", "Latiosite", "Roseli Berry", "Kee Berry", "Maranga Berry", "Sprinklotad",
			"TM96", "TM97", "TM98", "TM99", "TM100", "Power Plant Pass", "Mega Ring", "Intriguing Stone", "Common Stone", "Discount Coupon",
			"Elevator Key", "TMV Pass", "Honor of Kalos", "Adventure Rules", "Strange Souvenir", "Lens Case", "Makeup Bag", "Travel Trunk", "Lumiose Galette", "Shalour Sable",
			"Jaw Fossil", "Sail Fossil", "Looker Ticket", "Bike", "Holo Caster", "Fairy Gem", "Mega Charm", "Mega Glove", "Mach Bike", "Acro Bike",
			"Wailmer Pail", "Devon Parts", "Soot Sack", "Basement Key", "Pokéblock Kit", "Letter", "Eon Ticket", "Scanner", "Go-Goggles", "Meteorite",
			"Key to Room 1", "Key to Room 2", "Key to Room 4", "Key to Room 6", "Storage Key", "Devon Scope", "S.S. Ticket", "HM07", "Devon Scuba Gear", "Contest Costume",
			"Contest Costume", "Magma Suit", "Aqua Suit", "Pair of Tickets", "Mega Bracelet", "Mega Pendant", "Mega Glasses", "Mega Anchor", "Mega Stickpin", "Mega Tiara",
			"Mega Anklet", "Meteorite", "Swampertite", "Sceptilite", "Sablenite", "Altarianite", "Galladite", "Audinite", "Metagrossite", "Sharpedonite",
			"Slowbronite", "Steelixite", "Pidgeotite", "Glalitite", "Diancite", "Prison Bottle", "Mega Cuff", "Cameruptite", "Lopunnite", "Salamencite",
			"Beedrillite", "Meteorite", "Meteorite", "Key Stone", "Meteorite Shard", "Eon Flute", "n/a", "n/a", "n/a", "Electrium Z",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "Z-Ring", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"Electrium Z", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "UB Ball *", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a", "n/a",
			"n/a"
		});
		this.itembox1.FormattingEnabled = true;
		this.itembox1.Location = new System.Drawing.Point(78, 21);
		this.itembox1.Name = "itembox1";
		this.itembox1.Size = new System.Drawing.Size(107, 21);
		this.itembox1.TabIndex = 1;
		this.Label13.AutoSize = true;
		this.Label13.Location = new System.Drawing.Point(12, 24);
		this.Label13.Name = "Label13";
		this.Label13.Size = new System.Drawing.Size(36, 13);
		this.Label13.TabIndex = 521;
		this.Label13.Text = "Item 1";
		this.CardTitleRefinedBox.Enabled = false;
		this.CardTitleRefinedBox.Location = new System.Drawing.Point(90, 154);
		this.CardTitleRefinedBox.MaxLength = 12;
		this.CardTitleRefinedBox.Name = "CardTitleRefinedBox";
		this.CardTitleRefinedBox.Size = new System.Drawing.Size(378, 20);
		this.CardTitleRefinedBox.TabIndex = 2;
		this.CardTitleRefinedBox.TextChanged += new System.EventHandler(CardTitleRefinedBox_TextChanged);
		this.WC8_2_WR8_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.WC8_2_WR8_Button.Location = new System.Drawing.Point(4, 357);
		this.WC8_2_WR8_Button.Name = "WC8_2_WR8_Button";
		this.WC8_2_WR8_Button.Size = new System.Drawing.Size(113, 23);
		this.WC8_2_WR8_Button.TabIndex = 7;
		this.WC8_2_WR8_Button.Text = "Insert WB7 as WR7";
		this.WC8_2_WR8_Button.UseVisualStyleBackColor = true;
		this.WC8_2_WR8_Button.Visible = false;
		this.WC8_2_WR8_Button.Click += new System.EventHandler(WC8_2_WR8_Button_Click);
		this.label34.AutoSize = true;
		this.label34.Location = new System.Drawing.Point(308, 208);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(58, 13);
		this.label34.TabIndex = 709;
		this.label34.Text = "Timestamp";
		this.TimestampBox.Enabled = false;
		this.TimestampBox.Location = new System.Drawing.Point(381, 205);
		this.TimestampBox.MaxLength = 10;
		this.TimestampBox.Name = "TimestampBox";
		this.TimestampBox.Size = new System.Drawing.Size(87, 20);
		this.TimestampBox.TabIndex = 6;
		this.TimestampBox.TextChanged += new System.EventHandler(TimestampBox_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Location = new System.Drawing.Point(39, 19);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(29, 13);
		this.label19.TabIndex = 711;
		this.label19.Text = "Year";
		this.groupBox1.Controls.Add(this.DateNul);
		this.groupBox1.Controls.Add(this.SecBox);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.DayBox);
		this.groupBox1.Controls.Add(this.MinBox);
		this.groupBox1.Controls.Add(this.MonthBox);
		this.groupBox1.Controls.Add(this.HourBox);
		this.groupBox1.Controls.Add(this.YearBox);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Location = new System.Drawing.Point(537, 13);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(448, 70);
		this.groupBox1.TabIndex = 710;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Timestamp Reader";
		this.groupBox1.Visible = false;
		this.DateNul.Location = new System.Drawing.Point(395, 41);
		this.DateNul.Name = "DateNul";
		this.DateNul.Size = new System.Drawing.Size(41, 20);
		this.DateNul.TabIndex = 7;
		this.DateNul.Text = "NUL";
		this.DateNul.UseVisualStyleBackColor = true;
		this.DateNul.Click += new System.EventHandler(DateNul_Click);
		this.SecBox.Location = new System.Drawing.Point(324, 41);
		this.SecBox.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		this.SecBox.Name = "SecBox";
		this.SecBox.Size = new System.Drawing.Size(65, 20);
		this.SecBox.TabIndex = 6;
		this.SecBox.ValueChanged += new System.EventHandler(SecBox_ValueChanged);
		this.label40.AutoSize = true;
		this.label40.Location = new System.Drawing.Point(289, 43);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(26, 13);
		this.label40.TabIndex = 723;
		this.label40.Text = "Sec";
		this.DayBox.Location = new System.Drawing.Point(324, 17);
		this.DayBox.Maximum = new decimal(new int[4] { 31, 0, 0, 0 });
		this.DayBox.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.DayBox.Name = "DayBox";
		this.DayBox.Size = new System.Drawing.Size(65, 20);
		this.DayBox.TabIndex = 3;
		this.DayBox.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.DayBox.ValueChanged += new System.EventHandler(DayBox_ValueChanged);
		this.MinBox.Location = new System.Drawing.Point(205, 41);
		this.MinBox.Maximum = new decimal(new int[4] { 59, 0, 0, 0 });
		this.MinBox.Name = "MinBox";
		this.MinBox.Size = new System.Drawing.Size(65, 20);
		this.MinBox.TabIndex = 5;
		this.MinBox.ValueChanged += new System.EventHandler(MinBox_ValueChanged);
		this.MonthBox.Location = new System.Drawing.Point(205, 17);
		this.MonthBox.Maximum = new decimal(new int[4] { 12, 0, 0, 0 });
		this.MonthBox.Minimum = new decimal(new int[4] { 1, 0, 0, 0 });
		this.MonthBox.Name = "MonthBox";
		this.MonthBox.Size = new System.Drawing.Size(65, 20);
		this.MonthBox.TabIndex = 2;
		this.MonthBox.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.MonthBox.ValueChanged += new System.EventHandler(MonthBox_ValueChanged);
		this.HourBox.Location = new System.Drawing.Point(82, 41);
		this.HourBox.Maximum = new decimal(new int[4] { 23, 0, 0, 0 });
		this.HourBox.Name = "HourBox";
		this.HourBox.Size = new System.Drawing.Size(65, 20);
		this.HourBox.TabIndex = 4;
		this.HourBox.ValueChanged += new System.EventHandler(HourBox_ValueChanged);
		this.YearBox.Location = new System.Drawing.Point(82, 17);
		this.YearBox.Maximum = new decimal(new int[4] { 2060, 0, 0, 0 });
		this.YearBox.Minimum = new decimal(new int[4] { 2000, 0, 0, 0 });
		this.YearBox.Name = "YearBox";
		this.YearBox.Size = new System.Drawing.Size(65, 20);
		this.YearBox.TabIndex = 1;
		this.YearBox.Value = new decimal(new int[4] { 2000, 0, 0, 0 });
		this.YearBox.ValueChanged += new System.EventHandler(YearBox_ValueChanged);
		this.label39.AutoSize = true;
		this.label39.Location = new System.Drawing.Point(39, 43);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(30, 13);
		this.label39.TabIndex = 717;
		this.label39.Text = "Hour";
		this.label36.AutoSize = true;
		this.label36.Location = new System.Drawing.Point(289, 19);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(26, 13);
		this.label36.TabIndex = 714;
		this.label36.Text = "Day";
		this.label38.AutoSize = true;
		this.label38.Location = new System.Drawing.Point(162, 43);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(24, 13);
		this.label38.TabIndex = 718;
		this.label38.Text = "Min";
		this.label35.AutoSize = true;
		this.label35.Location = new System.Drawing.Point(162, 19);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(37, 13);
		this.label35.TabIndex = 712;
		this.label35.Text = "Month";
		this.InsertWR8Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.InsertWR8Button.Location = new System.Drawing.Point(123, 357);
		this.InsertWR8Button.Name = "InsertWR8Button";
		this.InsertWR8Button.Size = new System.Drawing.Size(113, 23);
		this.InsertWR8Button.TabIndex = 8;
		this.InsertWR8Button.Text = "Insert WR7";
		this.InsertWR8Button.UseVisualStyleBackColor = true;
		this.InsertWR8Button.Visible = false;
		this.InsertWR8Button.Click += new System.EventHandler(InsertWR8Button_Click);
		this.ExtractWR8Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.ExtractWR8Button.Location = new System.Drawing.Point(242, 357);
		this.ExtractWR8Button.Name = "ExtractWR8Button";
		this.ExtractWR8Button.Size = new System.Drawing.Size(113, 23);
		this.ExtractWR8Button.TabIndex = 9;
		this.ExtractWR8Button.Text = "Extract WR7";
		this.ExtractWR8Button.UseVisualStyleBackColor = true;
		this.ExtractWR8Button.Visible = false;
		this.ExtractWR8Button.Click += new System.EventHandler(ExtractWR8Button_Click);
		this.DeleteWR8Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.DeleteWR8Button.Location = new System.Drawing.Point(388, 357);
		this.DeleteWR8Button.Name = "DeleteWR8Button";
		this.DeleteWR8Button.Size = new System.Drawing.Size(113, 23);
		this.DeleteWR8Button.TabIndex = 10;
		this.DeleteWR8Button.Text = "Delete Slot";
		this.DeleteWR8Button.UseVisualStyleBackColor = true;
		this.DeleteWR8Button.Visible = false;
		this.DeleteWR8Button.Click += new System.EventHandler(DeleteWR8Button_Click);
		this.pictureBox10.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox10.Location = new System.Drawing.Point(436, 57);
		this.pictureBox10.Name = "pictureBox10";
		this.pictureBox10.Size = new System.Drawing.Size(68, 56);
		this.pictureBox10.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox10.TabIndex = 730;
		this.pictureBox10.TabStop = false;
		this.pictureBox10.Click += new System.EventHandler(Border_Change);
		this.pictureBox9.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox9.Location = new System.Drawing.Point(369, 57);
		this.pictureBox9.Name = "pictureBox9";
		this.pictureBox9.Size = new System.Drawing.Size(68, 56);
		this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox9.TabIndex = 729;
		this.pictureBox9.TabStop = false;
		this.pictureBox9.Click += new System.EventHandler(Border_Change);
		this.pictureBox8.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox8.Location = new System.Drawing.Point(302, 57);
		this.pictureBox8.Name = "pictureBox8";
		this.pictureBox8.Size = new System.Drawing.Size(68, 56);
		this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox8.TabIndex = 728;
		this.pictureBox8.TabStop = false;
		this.pictureBox8.Click += new System.EventHandler(Border_Change);
		this.pictureBox7.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox7.Location = new System.Drawing.Point(235, 57);
		this.pictureBox7.Name = "pictureBox7";
		this.pictureBox7.Size = new System.Drawing.Size(69, 56);
		this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox7.TabIndex = 727;
		this.pictureBox7.TabStop = false;
		this.pictureBox7.Click += new System.EventHandler(Border_Change);
		this.pictureBox6.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox6.Location = new System.Drawing.Point(168, 57);
		this.pictureBox6.Name = "pictureBox6";
		this.pictureBox6.Size = new System.Drawing.Size(68, 56);
		this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox6.TabIndex = 726;
		this.pictureBox6.TabStop = false;
		this.pictureBox6.Click += new System.EventHandler(Border_Change);
		this.pictureBox5.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox5.Location = new System.Drawing.Point(436, 2);
		this.pictureBox5.Name = "pictureBox5";
		this.pictureBox5.Size = new System.Drawing.Size(68, 56);
		this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox5.TabIndex = 725;
		this.pictureBox5.TabStop = false;
		this.pictureBox5.Click += new System.EventHandler(Border_Change);
		this.pictureBox4.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox4.Location = new System.Drawing.Point(369, 2);
		this.pictureBox4.Name = "pictureBox4";
		this.pictureBox4.Size = new System.Drawing.Size(68, 56);
		this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox4.TabIndex = 724;
		this.pictureBox4.TabStop = false;
		this.pictureBox4.Click += new System.EventHandler(Border_Change);
		this.pictureBox3.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox3.Location = new System.Drawing.Point(302, 2);
		this.pictureBox3.Name = "pictureBox3";
		this.pictureBox3.Size = new System.Drawing.Size(68, 56);
		this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox3.TabIndex = 723;
		this.pictureBox3.TabStop = false;
		this.pictureBox3.Click += new System.EventHandler(Border_Change);
		this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox2.Location = new System.Drawing.Point(235, 2);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(68, 56);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox2.TabIndex = 722;
		this.pictureBox2.TabStop = false;
		this.pictureBox2.Click += new System.EventHandler(Border_Change);
		this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox1.Location = new System.Drawing.Point(168, 2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(68, 56);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox1.TabIndex = 721;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Click += new System.EventHandler(Border_Change);
		this.pictureBox0.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.pictureBox0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox0.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pictureBox0.Location = new System.Drawing.Point(168, 112);
		this.pictureBox0.Name = "pictureBox0";
		this.pictureBox0.Size = new System.Drawing.Size(68, 56);
		this.pictureBox0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.pictureBox0.TabIndex = 731;
		this.pictureBox0.TabStop = false;
		this.pictureBox0.Visible = false;
		this.AllowDrop = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(520, 383);
		base.Controls.Add(this.pictureBox0);
		base.Controls.Add(this.pictureBox10);
		base.Controls.Add(this.pictureBox9);
		base.Controls.Add(this.pictureBox8);
		base.Controls.Add(this.pictureBox7);
		base.Controls.Add(this.pictureBox6);
		base.Controls.Add(this.pictureBox5);
		base.Controls.Add(this.pictureBox4);
		base.Controls.Add(this.pictureBox3);
		base.Controls.Add(this.pictureBox2);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.SpeciesImageBox);
		base.Controls.Add(this.DeleteWR8Button);
		base.Controls.Add(this.ExtractWR8Button);
		base.Controls.Add(this.InsertWR8Button);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.TimestampBox);
		base.Controls.Add(this.label34);
		base.Controls.Add(this.WC8_2_WR8_Button);
		base.Controls.Add(this.CardTitleRefinedBox);
		base.Controls.Add(this.WRTabs);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.CardTitleRawBox);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.WCIDBox);
		base.Controls.Add(this.EntryTypeBox);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.SlotIndex);
		base.Controls.Add(this.AreaSlotLabel);
		base.Controls.Add(this.LocationLabel);
		base.MaximizeBox = false;
		base.Name = "WonderRecordLGPE";
		base.ShowIcon = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Wonder Records Tool (LGPE)";
		base.Load += new System.EventHandler(WonderRecord_Load);
		((System.ComponentModel.ISupportInitialize)this.SlotIndex).EndInit();
		((System.ComponentModel.ISupportInitialize)this.FormBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.SpeciesImageBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.WCIDBox).EndInit();
		this.WRTabs.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.tabPage3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.SecBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.DayBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MinBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.MonthBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.HourBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.YearBox).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox10).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox9).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox8).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox7).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox6).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox5).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox4).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox0).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}