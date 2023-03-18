using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WC3Tool;

public class TV_editor : Form
{
	private SAV3 sav3file;

	public string savfilter = MainScreen.savfilter;

	private TV_EVENTS events;

	private SHOW ingame_swarm;

	private TV_SHOWS shows;

	private SWARM swarm;

	private IContainer components;

	private Button save_but;

	private GroupBox groupBox1;

	private NumericUpDown event_days;

	private ComboBox event_id;

	private ComboBox event_status;

	private NumericUpDown event_slot;

	private Label label1;

	private Label label5;

	private Label label4;

	private Label label3;

	private Label label2;

	private GroupBox groupBox2;

	private NumericUpDown current_appearance;

	private NumericUpDown current_map;

	private NumericUpDown current_level;

	private ComboBox current_species;

	private ComboBox current_move4;

	private ComboBox current_move3;

	private ComboBox current_move2;

	private ComboBox current_move1;

	private NumericUpDown current_remaining;

	private Label label14;

	private Label label10;

	private Label label11;

	private Label label12;

	private Label label13;

	private Label label9;

	private Label label8;

	private Label label7;

	private Label label6;

	private GroupBox groupBox3;

	private Button swarm_delete;

	private NumericUpDown tv_mix_tid;

	private Label label15;

	private NumericUpDown tv_tid;

	private Label label16;

	private NumericUpDown tv_id;

	private Label label17;

	private Label label18;

	private ComboBox tv_status;

	private NumericUpDown tv_slot;

	private Label label19;

	private Label label20;

	private GroupBox tv_outbreak_group;

	private NumericUpDown outbreak_activation;

	private Label label31;

	private Label label28;

	private Label label29;

	private Label label24;

	private NumericUpDown outbreak_level;

	private NumericUpDown outbreak_remaining;

	private ComboBox outbreak_species;

	private Label label25;

	private Label label23;

	private Label label26;

	private Label label21;

	private Label label27;

	private ComboBox outbreak_move4;

	private NumericUpDown outbreak_map;

	private ComboBox outbreak_move3;

	private Label label22;

	private ComboBox outbreak_move2;

	private NumericUpDown outbreak_availability;

	private ComboBox outbreak_move1;

	private Button outbreak_apply;

	private Button swarm_apply;

	private Button event_apply;

	private Label label30;

	private Label label32;

	public TV_editor(SAV3 save)
	{
		InitializeComponent();
		sav3file = save;
		events = new TV_EVENTS(sav3file.get_TV_EVENT());
		ingame_swarm = new SHOW(sav3file.get_TV_OUTBREAK());
		shows = new TV_SHOWS(sav3file.get_TV_SHOWS());
		swarm = new SWARM(sav3file.get_TV_OUTBREAK_EXTRA());
		load_data();
	}

	private void load_data()
	{
		load_event();
		load_swarm();
		load_show();
	}

	private void load_event()
	{
		event_id.SelectedIndex = events.events[(int)event_slot.Value].ID;
		event_status.SelectedIndex = events.events[(int)event_slot.Value].status;
		event_days.Value = events.events[(int)event_slot.Value].days_to_tv;
	}

	private void set_event()
	{
		events.events[(int)event_slot.Value].ID = (byte)event_id.SelectedIndex;
		events.events[(int)event_slot.Value].status = (byte)event_status.SelectedIndex;
		events.events[(int)event_slot.Value].days_to_tv = (ushort)event_days.Value;
	}

	private void load_swarm()
	{
		current_species.SelectedIndex = swarm.species;
		current_level.Value = swarm.level;
		current_move1.SelectedIndex = swarm.move1;
		current_move2.SelectedIndex = swarm.move2;
		current_move3.SelectedIndex = swarm.move3;
		current_move4.SelectedIndex = swarm.move4;
		current_map.Value = swarm.map;
		current_appearance.Value = swarm.appearance;
		current_remaining.Value = swarm.remaining_days;
	}

	private void set_swarm()
	{
		swarm.species = (ushort)current_species.SelectedIndex;
		swarm.level = (byte)current_level.Value;
		swarm.move1 = (ushort)current_move1.SelectedIndex;
		swarm.move2 = (ushort)current_move2.SelectedIndex;
		swarm.move3 = (ushort)current_move3.SelectedIndex;
		swarm.move4 = (ushort)current_move4.SelectedIndex;
		swarm.map = (ushort)current_map.Value;
		swarm.appearance = (byte)current_appearance.Value;
		swarm.remaining_days = (byte)current_remaining.Value;
	}

	private void load_show()
	{
		if (tv_slot.Value == 0m)
		{
			tv_id.Value = ingame_swarm.ID;
			tv_status.SelectedIndex = ingame_swarm.status;
			tv_tid.Value = ingame_swarm.TID_own;
			tv_mix_tid.Value = ingame_swarm.TID_mixed;
		}
		else
		{
			tv_id.Value = shows.shows[(int)(tv_slot.Value - 1m)].ID;
			tv_status.SelectedIndex = shows.shows[(int)(tv_slot.Value - 1m)].status;
			tv_tid.Value = shows.shows[(int)(tv_slot.Value - 1m)].TID_own;
			tv_mix_tid.Value = shows.shows[(int)(tv_slot.Value - 1m)].TID_mixed;
		}
		load_outbreak();
	}

	private void Save_butClick(object sender, EventArgs e)
	{
		events.set_events();
		shows.set_shows();
		sav3file.set_TV_EVENT(events.Data);
		sav3file.set_TV_OUTBREAK(ingame_swarm.Data);
		sav3file.set_TV_SHOWS(shows.Data);
		sav3file.set_TV_OUTBREAK_EXTRA(swarm.Data);
		sav3file.update_section_chk(3);
		FileIO.save_data(sav3file.Data, savfilter);
	}

	private void Event_slotValueChanged(object sender, EventArgs e)
	{
		load_event();
	}

	private void Swarm_deleteClick(object sender, EventArgs e)
	{
		delete_current_swarm();
		load_swarm();
	}

	private void delete_current_swarm()
	{
		int num = 0;
		for (num = 0; num < SWARM.swarm_size; num++)
		{
			swarm.Data[num] = 0;
		}
	}

	private void load_outbreak()
	{
		if (tv_slot.Value == 0m)
		{
			tv_outbreak_group.Enabled = true;
			outbreak_activation.Value = ingame_swarm.outbreak_days_to_tv;
			outbreak_map.Value = ingame_swarm.outbreak_map;
			outbreak_availability.Value = ingame_swarm.outbreak_appearance;
			outbreak_remaining.Value = ingame_swarm.outbreak_appearance;
			outbreak_species.SelectedIndex = ingame_swarm.outbreak_species;
			outbreak_level.Value = ingame_swarm.outbreak_level;
			outbreak_move1.SelectedIndex = ingame_swarm.outbreak_move1;
			outbreak_move2.SelectedIndex = ingame_swarm.outbreak_move2;
			outbreak_move3.SelectedIndex = ingame_swarm.outbreak_move3;
			outbreak_move4.SelectedIndex = ingame_swarm.outbreak_move4;
		}
		else if (tv_slot.Value != 0m && tv_id.Value == 41m)
		{
			tv_outbreak_group.Enabled = true;
			outbreak_activation.Value = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_days_to_tv;
			outbreak_map.Value = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_map;
			outbreak_availability.Value = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_appearance;
			outbreak_remaining.Value = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_appearance;
			outbreak_species.SelectedIndex = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_species;
			outbreak_level.Value = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_level;
			outbreak_move1.SelectedIndex = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_move1;
			outbreak_move2.SelectedIndex = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_move2;
			outbreak_move3.SelectedIndex = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_move3;
			outbreak_move4.SelectedIndex = shows.shows[(int)(tv_slot.Value - 1m)].outbreak_move4;
		}
		else
		{
			tv_outbreak_group.Enabled = false;
		}
	}

	private void Outbreak_applyClick(object sender, EventArgs e)
	{
		if (tv_slot.Value == 0m)
		{
			ingame_swarm.outbreak_days_to_tv = (ushort)outbreak_activation.Value;
			ingame_swarm.outbreak_map = (ushort)outbreak_map.Value;
			ingame_swarm.outbreak_appearance = (byte)outbreak_availability.Value;
			ingame_swarm.outbreak_appearance = (byte)outbreak_remaining.Value;
			ingame_swarm.outbreak_species = (ushort)outbreak_species.SelectedIndex;
			ingame_swarm.outbreak_level = (byte)outbreak_level.Value;
			ingame_swarm.outbreak_move1 = (ushort)outbreak_move1.SelectedIndex;
			ingame_swarm.outbreak_move2 = (ushort)outbreak_move2.SelectedIndex;
			ingame_swarm.outbreak_move3 = (ushort)outbreak_move3.SelectedIndex;
			ingame_swarm.outbreak_move4 = (ushort)outbreak_move4.SelectedIndex;
		}
		else if (tv_slot.Value != 0m && tv_id.Value == 41m)
		{
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_days_to_tv = (ushort)outbreak_activation.Value;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_map = (ushort)outbreak_map.Value;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_appearance = (byte)outbreak_availability.Value;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_appearance = (byte)outbreak_remaining.Value;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_species = (ushort)outbreak_species.SelectedIndex;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_level = (byte)outbreak_level.Value;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_move1 = (ushort)outbreak_move1.SelectedIndex;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_move2 = (ushort)outbreak_move2.SelectedIndex;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_move3 = (ushort)outbreak_move3.SelectedIndex;
			shows.shows[(int)(tv_slot.Value - 1m)].outbreak_move4 = (ushort)outbreak_move4.SelectedIndex;
		}
		MessageBox.Show("Outbreak Show Updated!");
	}

	private void Tv_statusSelectedIndexChanged(object sender, EventArgs e)
	{
		if (tv_slot.Value == 0m)
		{
			ingame_swarm.status = (byte)tv_status.SelectedIndex;
		}
		else
		{
			shows.shows[(int)(tv_slot.Value - 1m)].status = (byte)tv_status.SelectedIndex;
		}
	}

	private void Tv_tidValueChanged(object sender, EventArgs e)
	{
		if (tv_slot.Value == 0m)
		{
			ingame_swarm.TID_own = (ushort)tv_tid.Value;
		}
		else
		{
			shows.shows[(int)(tv_slot.Value - 1m)].TID_own = (ushort)tv_tid.Value;
		}
	}

	private void Tv_mix_tidValueChanged(object sender, EventArgs e)
	{
		if (tv_slot.Value == 0m)
		{
			ingame_swarm.TID_mixed = (ushort)tv_mix_tid.Value;
		}
		else
		{
			shows.shows[(int)(tv_slot.Value - 1m)].TID_mixed = (ushort)tv_mix_tid.Value;
		}
	}

	private void Tv_idValueChanged(object sender, EventArgs e)
	{
		if (tv_slot.Value == 0m)
		{
			tv_id.Value = ingame_swarm.ID;
		}
		else
		{
			tv_id.Value = shows.shows[(int)(tv_slot.Value - 1m)].ID;
		}
	}

	private void Tv_slotValueChanged(object sender, EventArgs e)
	{
		load_show();
	}

	private void Event_statusSelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void Event_idSelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void Event_daysValueChanged(object sender, EventArgs e)
	{
	}

	private void Swarm_applyClick(object sender, EventArgs e)
	{
		set_swarm();
	}

	private void Event_applyClick(object sender, EventArgs e)
	{
		set_event();
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
		this.save_but = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.event_apply = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.event_days = new System.Windows.Forms.NumericUpDown();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.event_id = new System.Windows.Forms.ComboBox();
		this.event_status = new System.Windows.Forms.ComboBox();
		this.event_slot = new System.Windows.Forms.NumericUpDown();
		this.label1 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.swarm_apply = new System.Windows.Forms.Button();
		this.swarm_delete = new System.Windows.Forms.Button();
		this.current_remaining = new System.Windows.Forms.NumericUpDown();
		this.label14 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.current_appearance = new System.Windows.Forms.NumericUpDown();
		this.current_map = new System.Windows.Forms.NumericUpDown();
		this.current_level = new System.Windows.Forms.NumericUpDown();
		this.current_species = new System.Windows.Forms.ComboBox();
		this.current_move4 = new System.Windows.Forms.ComboBox();
		this.current_move3 = new System.Windows.Forms.ComboBox();
		this.current_move2 = new System.Windows.Forms.ComboBox();
		this.current_move1 = new System.Windows.Forms.ComboBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.tv_id = new System.Windows.Forms.NumericUpDown();
		this.label20 = new System.Windows.Forms.Label();
		this.tv_mix_tid = new System.Windows.Forms.NumericUpDown();
		this.label15 = new System.Windows.Forms.Label();
		this.tv_tid = new System.Windows.Forms.NumericUpDown();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.tv_status = new System.Windows.Forms.ComboBox();
		this.tv_slot = new System.Windows.Forms.NumericUpDown();
		this.label19 = new System.Windows.Forms.Label();
		this.tv_outbreak_group = new System.Windows.Forms.GroupBox();
		this.outbreak_apply = new System.Windows.Forms.Button();
		this.outbreak_activation = new System.Windows.Forms.NumericUpDown();
		this.label31 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.outbreak_level = new System.Windows.Forms.NumericUpDown();
		this.outbreak_remaining = new System.Windows.Forms.NumericUpDown();
		this.outbreak_species = new System.Windows.Forms.ComboBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.outbreak_move4 = new System.Windows.Forms.ComboBox();
		this.outbreak_map = new System.Windows.Forms.NumericUpDown();
		this.outbreak_move3 = new System.Windows.Forms.ComboBox();
		this.label22 = new System.Windows.Forms.Label();
		this.outbreak_move2 = new System.Windows.Forms.ComboBox();
		this.outbreak_availability = new System.Windows.Forms.NumericUpDown();
		this.outbreak_move1 = new System.Windows.Forms.ComboBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.event_days).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.event_slot).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.current_remaining).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.current_appearance).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.current_map).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.current_level).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.tv_id).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.tv_mix_tid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.tv_tid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.tv_slot).BeginInit();
		this.tv_outbreak_group.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.outbreak_activation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.outbreak_level).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.outbreak_remaining).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.outbreak_map).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.outbreak_availability).BeginInit();
		base.SuspendLayout();
		this.save_but.Location = new System.Drawing.Point(265, 511);
		this.save_but.Name = "save_but";
		this.save_but.Size = new System.Drawing.Size(75, 23);
		this.save_but.TabIndex = 7;
		this.save_but.Text = "Save";
		this.save_but.UseVisualStyleBackColor = true;
		this.save_but.Click += new System.EventHandler(Save_butClick);
		this.groupBox1.Controls.Add(this.event_apply);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.event_days);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.event_id);
		this.groupBox1.Controls.Add(this.event_status);
		this.groupBox1.Controls.Add(this.event_slot);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Location = new System.Drawing.Point(17, 12);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(383, 152);
		this.groupBox1.TabIndex = 8;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "TV Events";
		this.event_apply.Location = new System.Drawing.Point(271, 110);
		this.event_apply.Name = "event_apply";
		this.event_apply.Size = new System.Drawing.Size(97, 23);
		this.event_apply.TabIndex = 31;
		this.event_apply.Text = "Apply changes";
		this.event_apply.UseVisualStyleBackColor = true;
		this.event_apply.Click += new System.EventHandler(Event_applyClick);
		this.label5.Location = new System.Drawing.Point(6, 125);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(247, 18);
		this.label5.TabIndex = 8;
		this.label5.Text = "Note: annoucement starts 2 days before activation";
		this.event_days.Location = new System.Drawing.Point(114, 99);
		this.event_days.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.event_days.Name = "event_days";
		this.event_days.Size = new System.Drawing.Size(71, 20);
		this.event_days.TabIndex = 4;
		this.event_days.ValueChanged += new System.EventHandler(Event_daysValueChanged);
		this.label4.Location = new System.Drawing.Point(6, 101);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(115, 21);
		this.label4.TabIndex = 7;
		this.label4.Text = "Days until activation:";
		this.label3.Location = new System.Drawing.Point(6, 75);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(39, 21);
		this.label3.TabIndex = 6;
		this.label3.Text = "Event:";
		this.label2.Location = new System.Drawing.Point(6, 48);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 21);
		this.label2.TabIndex = 5;
		this.label2.Text = "Status:";
		this.event_id.FormattingEnabled = true;
		this.event_id.Items.AddRange(new object[5] { "None", "Big Sale (Slateport Energy Guru)", "Service Day (Mauville Game Corner)", "Clear-Out-Sale (Lilycove Department Store)", "Blend Master (Lilycove Contest Hall) (Emerald only!)" });
		this.event_id.Location = new System.Drawing.Point(114, 72);
		this.event_id.Name = "event_id";
		this.event_id.Size = new System.Drawing.Size(263, 21);
		this.event_id.TabIndex = 3;
		this.event_id.SelectedIndexChanged += new System.EventHandler(Event_idSelectedIndexChanged);
		this.event_status.FormattingEnabled = true;
		this.event_status.Items.AddRange(new object[3] { "Seen", "Not yet seen", "Seen + event active" });
		this.event_status.Location = new System.Drawing.Point(114, 45);
		this.event_status.Name = "event_status";
		this.event_status.Size = new System.Drawing.Size(263, 21);
		this.event_status.TabIndex = 2;
		this.event_status.SelectedIndexChanged += new System.EventHandler(Event_statusSelectedIndexChanged);
		this.event_slot.Location = new System.Drawing.Point(114, 19);
		this.event_slot.Maximum = new decimal(new int[4] { 15, 0, 0, 0 });
		this.event_slot.Name = "event_slot";
		this.event_slot.Size = new System.Drawing.Size(71, 20);
		this.event_slot.TabIndex = 0;
		this.event_slot.ValueChanged += new System.EventHandler(Event_slotValueChanged);
		this.label1.Location = new System.Drawing.Point(6, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(39, 21);
		this.label1.TabIndex = 1;
		this.label1.Text = "Slot:";
		this.groupBox2.Controls.Add(this.swarm_apply);
		this.groupBox2.Controls.Add(this.swarm_delete);
		this.groupBox2.Controls.Add(this.current_remaining);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.current_appearance);
		this.groupBox2.Controls.Add(this.current_map);
		this.groupBox2.Controls.Add(this.current_level);
		this.groupBox2.Controls.Add(this.current_species);
		this.groupBox2.Controls.Add(this.current_move4);
		this.groupBox2.Controls.Add(this.current_move3);
		this.groupBox2.Controls.Add(this.current_move2);
		this.groupBox2.Controls.Add(this.current_move1);
		this.groupBox2.Location = new System.Drawing.Point(406, 12);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(217, 310);
		this.groupBox2.TabIndex = 9;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Current Outbreak";
		this.swarm_apply.Location = new System.Drawing.Point(7, 275);
		this.swarm_apply.Name = "swarm_apply";
		this.swarm_apply.Size = new System.Drawing.Size(97, 23);
		this.swarm_apply.TabIndex = 30;
		this.swarm_apply.Text = "Apply changes";
		this.swarm_apply.UseVisualStyleBackColor = true;
		this.swarm_apply.Click += new System.EventHandler(Swarm_applyClick);
		this.swarm_delete.Location = new System.Drawing.Point(114, 275);
		this.swarm_delete.Name = "swarm_delete";
		this.swarm_delete.Size = new System.Drawing.Size(97, 23);
		this.swarm_delete.TabIndex = 18;
		this.swarm_delete.Text = "Delete";
		this.swarm_delete.UseVisualStyleBackColor = true;
		this.swarm_delete.Click += new System.EventHandler(Swarm_deleteClick);
		this.current_remaining.Location = new System.Drawing.Point(89, 227);
		this.current_remaining.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.current_remaining.Name = "current_remaining";
		this.current_remaining.Size = new System.Drawing.Size(120, 20);
		this.current_remaining.TabIndex = 8;
		this.label14.Location = new System.Drawing.Point(7, 224);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(86, 23);
		this.label14.TabIndex = 17;
		this.label14.Text = "Days remaining";
		this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label10.Location = new System.Drawing.Point(7, 202);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 23);
		this.label10.TabIndex = 16;
		this.label10.Text = "Availability %";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label11.Location = new System.Drawing.Point(7, 175);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(78, 23);
		this.label11.TabIndex = 15;
		this.label11.Text = "Map";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label12.Location = new System.Drawing.Point(7, 148);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 23);
		this.label12.TabIndex = 14;
		this.label12.Text = "Move 4";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label13.Location = new System.Drawing.Point(7, 122);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(78, 23);
		this.label13.TabIndex = 13;
		this.label13.Text = "Move 3";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label9.Location = new System.Drawing.Point(7, 96);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(78, 23);
		this.label9.TabIndex = 12;
		this.label9.Text = "Move 2";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label8.Location = new System.Drawing.Point(7, 69);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 23);
		this.label8.TabIndex = 11;
		this.label8.Text = "Move 1";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label7.Location = new System.Drawing.Point(7, 42);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 23);
		this.label7.TabIndex = 10;
		this.label7.Text = "Level";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label6.Location = new System.Drawing.Point(7, 16);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 23);
		this.label6.TabIndex = 9;
		this.label6.Text = "Species";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.current_appearance.Location = new System.Drawing.Point(89, 201);
		this.current_appearance.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.current_appearance.Name = "current_appearance";
		this.current_appearance.Size = new System.Drawing.Size(120, 20);
		this.current_appearance.TabIndex = 7;
		this.current_map.Location = new System.Drawing.Point(89, 175);
		this.current_map.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.current_map.Name = "current_map";
		this.current_map.Size = new System.Drawing.Size(120, 20);
		this.current_map.TabIndex = 6;
		this.current_level.Location = new System.Drawing.Point(91, 45);
		this.current_level.Name = "current_level";
		this.current_level.Size = new System.Drawing.Size(120, 20);
		this.current_level.TabIndex = 5;
		this.current_level.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.current_species.FormattingEnabled = true;
		this.current_species.Items.AddRange(new object[440]
		{
			"NONE", "Bulbasaur", "Ivysaur", "Venusaur", "Charmander", "Charmeleon", "Charizard", "Squirtle", "Wartortle", "Blastoise",
			"Caterpie", "Metapod", "Butterfree", "Weedle", "Kakuna", "Beedrill", "Pidgey", "Pidgeotto", "Pidgeot", "Rattata",
			"Raticate", "Spearow", "Fearow", "Ekans", "Arbok", "Pikachu", "Raichu", "Sandshrew", "Sandslash", "Nidoran♀",
			"Nidorina", "Nidoqueen", "Nidoran♂", "Nidorino", "Nidoking", "Clefairy", "Clefable", "Vulpix", "Ninetales", "Jigglypuff",
			"Wigglytuff", "Zubat", "Golbat", "Oddish", "Gloom", "Vileplume", "Paras", "Parasect", "Venonat", "Venomoth",
			"Diglett", "Dugtrio", "Meowth", "Persian", "Psyduck", "Golduck", "Mankey", "Primeape", "Growlithe", "Arcanine",
			"Poliwag", "Poliwhirl", "Poliwrath", "Abra", "Kadabra", "Alakazam", "Machop", "Machoke", "Machamp", "Bellsprout",
			"Weepinbell", "Victreebel", "Tentacool", "Tentacruel", "Geodude", "Graveler", "Golem", "Ponyta", "Rapidash", "Slowpoke",
			"Slowbro", "Magnemite", "Magneton", "Farfetch'd", "Doduo", "Dodrio", "Seel", "Dewgong", "Grimer", "Muk",
			"Shellder", "Cloyster", "Gastly", "Haunter", "Gengar", "Onix", "Drowzee", "Hypno", "Krabby", "Kingler",
			"Voltorb", "Electrode", "Exeggcute", "Exeggutor", "Cubone", "Marowak", "Hitmonlee", "Hitmonchan", "Lickitung", "Koffing",
			"Weezing", "Rhyhorn", "Rhydon", "Chansey", "Tangela", "Kangaskhan", "Horsea", "Seadra", "Goldeen", "Seaking",
			"Staryu", "Starmie", "Mr. Mime", "Scyther", "Jynx", "Electabuzz", "Magmar", "Pinsir", "Tauros", "Magikarp",
			"Gyarados", "Lapras", "Ditto", "Eevee", "Vaporeon", "Jolteon", "Flareon", "Porygon", "Omanyte", "Omastar",
			"Kabuto", "Kabutops", "Aerodactyl", "Snorlax", "Articuno", "Zapdos", "Moltres", "Dratini", "Dragonair", "Dragonite",
			"Mewtwo", "Mew", "Chikorita", "Bayleef", "Meganium", "Cyndaquil", "Quilava", "Typhlosion", "Totodile", "Croconaw",
			"Feraligatr", "Sentret", "Furret", "Hoothoot", "Noctowl", "Ledyba", "Ledian", "Spinarak", "Ariados", "Crobat",
			"Chinchou", "Lanturn", "Pichu", "Cleffa", "Igglybuff", "Togepi", "Togetic", "Natu", "Xatu", "Mareep",
			"Flaaffy", "Ampharos", "Bellossom", "Marill", "Azumarill", "Sudowoodo", "Politoed", "Hoppip", "Skiploom", "Jumpluff",
			"Aipom", "Sunkern", "Sunflora", "Yanma", "Wooper", "Quagsire", "Espeon", "Umbreon", "Murkrow", "Slowking",
			"Misdreavus", "Unown A", "Wobbuffet", "Girafarig", "Pineco", "Forretress", "Dunsparce", "Gligar", "Steelix", "Snubbull",
			"Granbull", "Qwilfish", "Scizor", "Shuckle", "Heracross", "Sneasel", "Teddiursa", "Ursaring", "Slugma", "Magcargo",
			"Swinub", "Piloswine", "Corsola", "Remoraid", "Octillery", "Delibird", "Mantine", "Skarmory", "Houndour", "Houndoom",
			"Kingdra", "Phanpy", "Donphan", "Porygon2", "Stantler", "Smeargle", "Tyrogue", "Hitmontop", "Smoochum", "Elekid",
			"Magby", "Miltank", "Blissey", "Raikou", "Entei", "Suicune", "Larvitar", "Pupitar", "Tyranitar", "Lugia",
			"Ho-oh", "Celebi", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)",
			"? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)",
			"? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "Treecko", "Grovyle", "Sceptile",
			"Torchic", "Combusken", "Blaziken", "Mudkip", "Marshtomp", "Swampert", "Poochyena", "Mightyena", "Zigzagoon", "Linoone",
			"Wurmple", "Silcoon", "Beautifly", "Cascoon", "Dustox", "Lotad", "Lombre", "Ludicolo", "Seedot", "Nuzleaf",
			"Shiftry", "Nincada", "Ninjask", "Shedinja", "Taillow", "Swellow", "Shroomish", "Breloom", "Spinda", "Wingull",
			"Pelipper", "Surskit", "Masquerain", "Wailmer", "Wailord", "Skitty", "Delcatty", "Kecleon", "Baltoy", "Claydol",
			"Nosepass", "Torkoal", "Sableye", "Barboach", "Whiscash", "Luvdisc", "Corphish", "Crawdaunt", "Feebas", "Milotic",
			"Carvanha", "Sharpedo", "Trapinch", "Vibrava", "Flygon", "Makuhita", "Hariyama", "Electrike", "Manectric", "Numel",
			"Camerupt", "Spheal", "Sealeo", "Walrein", "Cacnea", "Cacturne", "Snorunt", "Glalie", "Lunatone", "Solrock",
			"Azurill", "Spoink", "Grumpig", "Plusle", "Minun", "Mawile", "Meditite", "Medicham", "Swablu", "Altaria",
			"Wynaut", "Duskull", "Dusclops", "Roselia", "Slakoth", "Vigoroth", "Slaking", "Gulpin", "Swalot", "Tropius",
			"Whismur", "Loudred", "Exploud", "Clamperl", "Huntail", "Gorebyss", "Absol", "Shuppet", "Banette", "Seviper",
			"Zangoose", "Relicanth", "Aron", "Lairon", "Aggron", "Castform", "Volbeat", "Illumise", "Lileep", "Cradily",
			"Anorith", "Armaldo", "Ralts", "Kirlia", "Gardevoir", "Bagon", "Shelgon", "Salamence", "Beldum", "Metang",
			"Metagross", "Regirock", "Regice", "Registeel", "Kyogre", "Groudon", "Rayquaza", "Latias", "Latios", "Jirachi",
			"Deoxys", "Chimecho", "Pokémon Egg", "Unown B", "Unown C", "Unown D", "Unown E", "Unown F", "Unown G", "Unown H",
			"Unown I", "Unown J", "Unown K", "Unown L", "Unown M", "Unown N", "Unown O", "Unown P", "Unown Q", "Unown R",
			"Unown S", "Unown T", "Unown U", "Unown V", "Unown W", "Unown X", "Unown Y", "Unown Z", "Unown !", "Unown ?"
		});
		this.current_species.Location = new System.Drawing.Point(90, 18);
		this.current_species.Name = "current_species";
		this.current_species.Size = new System.Drawing.Size(121, 21);
		this.current_species.TabIndex = 4;
		this.current_move4.FormattingEnabled = true;
		this.current_move4.Items.AddRange(new object[355]
		{
			"-NONE-", "Pound", "Karate Chop*", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch",
			"Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust*", "Wing Attack", "Whirlwind", "Fly",
			"Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack*", "Headbutt",
			"Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip",
			"Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite*", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom",
			"Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard",
			"Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss",
			"Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder",
			"Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake",
			"Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage",
			"Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray",
			"Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move",
			"Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
			"Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas",
			"Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
			"Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
			"Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web",
			"Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse*", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal",
			"Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss*", "Belly Drum", "Sludge Bomb", "Mud-Slap",
			"Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On",
			"Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm*", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark",
			"Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard",
			"Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin",
			"Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight*", "Hidden Power", "Cross Chop", "Twister",
			"Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash",
			"Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment",
			"Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt",
			"Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge",
			"Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch",
			"Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick",
			"Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash",
			"Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound",
			"Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold",
			"Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up",
			"Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance",
			"Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost"
		});
		this.current_move4.Location = new System.Drawing.Point(89, 148);
		this.current_move4.Name = "current_move4";
		this.current_move4.Size = new System.Drawing.Size(121, 21);
		this.current_move4.TabIndex = 3;
		this.current_move3.FormattingEnabled = true;
		this.current_move3.Items.AddRange(new object[355]
		{
			"-NONE-", "Pound", "Karate Chop*", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch",
			"Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust*", "Wing Attack", "Whirlwind", "Fly",
			"Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack*", "Headbutt",
			"Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip",
			"Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite*", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom",
			"Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard",
			"Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss",
			"Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder",
			"Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake",
			"Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage",
			"Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray",
			"Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move",
			"Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
			"Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas",
			"Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
			"Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
			"Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web",
			"Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse*", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal",
			"Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss*", "Belly Drum", "Sludge Bomb", "Mud-Slap",
			"Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On",
			"Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm*", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark",
			"Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard",
			"Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin",
			"Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight*", "Hidden Power", "Cross Chop", "Twister",
			"Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash",
			"Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment",
			"Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt",
			"Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge",
			"Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch",
			"Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick",
			"Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash",
			"Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound",
			"Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold",
			"Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up",
			"Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance",
			"Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost"
		});
		this.current_move3.Location = new System.Drawing.Point(89, 124);
		this.current_move3.Name = "current_move3";
		this.current_move3.Size = new System.Drawing.Size(121, 21);
		this.current_move3.TabIndex = 2;
		this.current_move2.FormattingEnabled = true;
		this.current_move2.Items.AddRange(new object[355]
		{
			"-NONE-", "Pound", "Karate Chop*", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch",
			"Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust*", "Wing Attack", "Whirlwind", "Fly",
			"Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack*", "Headbutt",
			"Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip",
			"Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite*", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom",
			"Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard",
			"Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss",
			"Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder",
			"Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake",
			"Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage",
			"Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray",
			"Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move",
			"Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
			"Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas",
			"Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
			"Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
			"Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web",
			"Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse*", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal",
			"Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss*", "Belly Drum", "Sludge Bomb", "Mud-Slap",
			"Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On",
			"Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm*", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark",
			"Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard",
			"Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin",
			"Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight*", "Hidden Power", "Cross Chop", "Twister",
			"Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash",
			"Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment",
			"Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt",
			"Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge",
			"Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch",
			"Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick",
			"Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash",
			"Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound",
			"Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold",
			"Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up",
			"Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance",
			"Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost"
		});
		this.current_move2.Location = new System.Drawing.Point(89, 98);
		this.current_move2.Name = "current_move2";
		this.current_move2.Size = new System.Drawing.Size(121, 21);
		this.current_move2.TabIndex = 1;
		this.current_move1.FormattingEnabled = true;
		this.current_move1.Items.AddRange(new object[355]
		{
			"-NONE-", "Pound", "Karate Chop*", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch",
			"Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust*", "Wing Attack", "Whirlwind", "Fly",
			"Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack*", "Headbutt",
			"Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip",
			"Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite*", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom",
			"Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard",
			"Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss",
			"Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder",
			"Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake",
			"Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage",
			"Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray",
			"Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move",
			"Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
			"Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas",
			"Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
			"Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
			"Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web",
			"Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse*", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal",
			"Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss*", "Belly Drum", "Sludge Bomb", "Mud-Slap",
			"Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On",
			"Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm*", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark",
			"Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard",
			"Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin",
			"Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight*", "Hidden Power", "Cross Chop", "Twister",
			"Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash",
			"Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment",
			"Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt",
			"Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge",
			"Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch",
			"Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick",
			"Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash",
			"Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound",
			"Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold",
			"Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up",
			"Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance",
			"Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost"
		});
		this.current_move1.Location = new System.Drawing.Point(89, 71);
		this.current_move1.Name = "current_move1";
		this.current_move1.Size = new System.Drawing.Size(121, 21);
		this.current_move1.TabIndex = 0;
		this.groupBox3.Controls.Add(this.tv_id);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.tv_mix_tid);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.tv_tid);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.tv_status);
		this.groupBox3.Controls.Add(this.tv_slot);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Location = new System.Drawing.Point(17, 170);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(383, 152);
		this.groupBox3.TabIndex = 9;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "TV Shows";
		this.tv_id.Hexadecimal = true;
		this.tv_id.Location = new System.Drawing.Point(114, 43);
		this.tv_id.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.tv_id.Name = "tv_id";
		this.tv_id.ReadOnly = true;
		this.tv_id.Size = new System.Drawing.Size(71, 20);
		this.tv_id.TabIndex = 17;
		this.tv_id.ValueChanged += new System.EventHandler(Tv_idValueChanged);
		this.label20.Location = new System.Drawing.Point(201, 17);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(167, 42);
		this.label20.TabIndex = 16;
		this.label20.Text = "Note: slot 0 corresponds to the in-game generated Pokémon outbreak TV show";
		this.tv_mix_tid.Location = new System.Drawing.Point(114, 120);
		this.tv_mix_tid.Maximum = new decimal(new int[4] { 65535, 0, 0, 0 });
		this.tv_mix_tid.Name = "tv_mix_tid";
		this.tv_mix_tid.Size = new System.Drawing.Size(71, 20);
		this.tv_mix_tid.TabIndex = 14;
		this.tv_mix_tid.ValueChanged += new System.EventHandler(Tv_mix_tidValueChanged);
		this.label15.Location = new System.Drawing.Point(6, 122);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(102, 21);
		this.label15.TabIndex = 15;
		this.label15.Text = "Record Mixing TID:";
		this.tv_tid.Location = new System.Drawing.Point(114, 96);
		this.tv_tid.Maximum = new decimal(new int[4] { 65535, 0, 0, 0 });
		this.tv_tid.Name = "tv_tid";
		this.tv_tid.Size = new System.Drawing.Size(71, 20);
		this.tv_tid.TabIndex = 12;
		this.tv_tid.ValueChanged += new System.EventHandler(Tv_tidValueChanged);
		this.label16.Location = new System.Drawing.Point(6, 98);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(90, 21);
		this.label16.TabIndex = 13;
		this.label16.Text = "Game TID:";
		this.label17.Location = new System.Drawing.Point(6, 45);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(90, 21);
		this.label17.TabIndex = 10;
		this.label17.Text = "ID (hexadecimal):";
		this.label18.Location = new System.Drawing.Point(6, 71);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(39, 21);
		this.label18.TabIndex = 5;
		this.label18.Text = "Status:";
		this.tv_status.FormattingEnabled = true;
		this.tv_status.Items.AddRange(new object[2] { "Seen", "Not yet seen" });
		this.tv_status.Location = new System.Drawing.Point(114, 69);
		this.tv_status.Name = "tv_status";
		this.tv_status.Size = new System.Drawing.Size(263, 21);
		this.tv_status.TabIndex = 2;
		this.tv_status.SelectedIndexChanged += new System.EventHandler(Tv_statusSelectedIndexChanged);
		this.tv_slot.Location = new System.Drawing.Point(114, 19);
		this.tv_slot.Maximum = new decimal(new int[4] { 7, 0, 0, 0 });
		this.tv_slot.Name = "tv_slot";
		this.tv_slot.Size = new System.Drawing.Size(71, 20);
		this.tv_slot.TabIndex = 0;
		this.tv_slot.ValueChanged += new System.EventHandler(Tv_slotValueChanged);
		this.label19.Location = new System.Drawing.Point(6, 21);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(39, 21);
		this.label19.TabIndex = 1;
		this.label19.Text = "Slot:";
		this.tv_outbreak_group.Controls.Add(this.outbreak_apply);
		this.tv_outbreak_group.Controls.Add(this.outbreak_activation);
		this.tv_outbreak_group.Controls.Add(this.label31);
		this.tv_outbreak_group.Controls.Add(this.label28);
		this.tv_outbreak_group.Controls.Add(this.label29);
		this.tv_outbreak_group.Controls.Add(this.label24);
		this.tv_outbreak_group.Controls.Add(this.outbreak_level);
		this.tv_outbreak_group.Controls.Add(this.outbreak_remaining);
		this.tv_outbreak_group.Controls.Add(this.outbreak_species);
		this.tv_outbreak_group.Controls.Add(this.label25);
		this.tv_outbreak_group.Controls.Add(this.label23);
		this.tv_outbreak_group.Controls.Add(this.label26);
		this.tv_outbreak_group.Controls.Add(this.label21);
		this.tv_outbreak_group.Controls.Add(this.label27);
		this.tv_outbreak_group.Controls.Add(this.outbreak_move4);
		this.tv_outbreak_group.Controls.Add(this.outbreak_map);
		this.tv_outbreak_group.Controls.Add(this.outbreak_move3);
		this.tv_outbreak_group.Controls.Add(this.label22);
		this.tv_outbreak_group.Controls.Add(this.outbreak_move2);
		this.tv_outbreak_group.Controls.Add(this.outbreak_availability);
		this.tv_outbreak_group.Controls.Add(this.outbreak_move1);
		this.tv_outbreak_group.Enabled = false;
		this.tv_outbreak_group.Location = new System.Drawing.Point(17, 328);
		this.tv_outbreak_group.Name = "tv_outbreak_group";
		this.tv_outbreak_group.Size = new System.Drawing.Size(606, 177);
		this.tv_outbreak_group.TabIndex = 10;
		this.tv_outbreak_group.TabStop = false;
		this.tv_outbreak_group.Text = "TV Outbreak Show Editing";
		this.outbreak_apply.Location = new System.Drawing.Point(55, 133);
		this.outbreak_apply.Name = "outbreak_apply";
		this.outbreak_apply.Size = new System.Drawing.Size(106, 23);
		this.outbreak_apply.TabIndex = 29;
		this.outbreak_apply.Text = "Apply changes";
		this.outbreak_apply.UseVisualStyleBackColor = true;
		this.outbreak_apply.Click += new System.EventHandler(Outbreak_applyClick);
		this.outbreak_activation.Location = new System.Drawing.Point(114, 19);
		this.outbreak_activation.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.outbreak_activation.Name = "outbreak_activation";
		this.outbreak_activation.Size = new System.Drawing.Size(120, 20);
		this.outbreak_activation.TabIndex = 27;
		this.label31.Location = new System.Drawing.Point(6, 21);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(115, 21);
		this.label31.TabIndex = 28;
		this.label31.Text = "Days until activation:";
		this.label28.Location = new System.Drawing.Point(329, 42);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(78, 23);
		this.label28.TabIndex = 22;
		this.label28.Text = "Level";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label29.Location = new System.Drawing.Point(329, 16);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(78, 23);
		this.label29.TabIndex = 21;
		this.label29.Text = "Species";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label24.Location = new System.Drawing.Point(330, 148);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(78, 23);
		this.label24.TabIndex = 26;
		this.label24.Text = "Move 4";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.outbreak_level.Location = new System.Drawing.Point(413, 45);
		this.outbreak_level.Name = "outbreak_level";
		this.outbreak_level.Size = new System.Drawing.Size(120, 20);
		this.outbreak_level.TabIndex = 20;
		this.outbreak_level.Value = new decimal(new int[4] { 1, 0, 0, 0 });
		this.outbreak_remaining.Location = new System.Drawing.Point(114, 97);
		this.outbreak_remaining.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.outbreak_remaining.Name = "outbreak_remaining";
		this.outbreak_remaining.Size = new System.Drawing.Size(120, 20);
		this.outbreak_remaining.TabIndex = 21;
		this.outbreak_species.FormattingEnabled = true;
		this.outbreak_species.Items.AddRange(new object[440]
		{
			"NONE", "Bulbasaur", "Ivysaur", "Venusaur", "Charmander", "Charmeleon", "Charizard", "Squirtle", "Wartortle", "Blastoise",
			"Caterpie", "Metapod", "Butterfree", "Weedle", "Kakuna", "Beedrill", "Pidgey", "Pidgeotto", "Pidgeot", "Rattata",
			"Raticate", "Spearow", "Fearow", "Ekans", "Arbok", "Pikachu", "Raichu", "Sandshrew", "Sandslash", "Nidoran♀",
			"Nidorina", "Nidoqueen", "Nidoran♂", "Nidorino", "Nidoking", "Clefairy", "Clefable", "Vulpix", "Ninetales", "Jigglypuff",
			"Wigglytuff", "Zubat", "Golbat", "Oddish", "Gloom", "Vileplume", "Paras", "Parasect", "Venonat", "Venomoth",
			"Diglett", "Dugtrio", "Meowth", "Persian", "Psyduck", "Golduck", "Mankey", "Primeape", "Growlithe", "Arcanine",
			"Poliwag", "Poliwhirl", "Poliwrath", "Abra", "Kadabra", "Alakazam", "Machop", "Machoke", "Machamp", "Bellsprout",
			"Weepinbell", "Victreebel", "Tentacool", "Tentacruel", "Geodude", "Graveler", "Golem", "Ponyta", "Rapidash", "Slowpoke",
			"Slowbro", "Magnemite", "Magneton", "Farfetch'd", "Doduo", "Dodrio", "Seel", "Dewgong", "Grimer", "Muk",
			"Shellder", "Cloyster", "Gastly", "Haunter", "Gengar", "Onix", "Drowzee", "Hypno", "Krabby", "Kingler",
			"Voltorb", "Electrode", "Exeggcute", "Exeggutor", "Cubone", "Marowak", "Hitmonlee", "Hitmonchan", "Lickitung", "Koffing",
			"Weezing", "Rhyhorn", "Rhydon", "Chansey", "Tangela", "Kangaskhan", "Horsea", "Seadra", "Goldeen", "Seaking",
			"Staryu", "Starmie", "Mr. Mime", "Scyther", "Jynx", "Electabuzz", "Magmar", "Pinsir", "Tauros", "Magikarp",
			"Gyarados", "Lapras", "Ditto", "Eevee", "Vaporeon", "Jolteon", "Flareon", "Porygon", "Omanyte", "Omastar",
			"Kabuto", "Kabutops", "Aerodactyl", "Snorlax", "Articuno", "Zapdos", "Moltres", "Dratini", "Dragonair", "Dragonite",
			"Mewtwo", "Mew", "Chikorita", "Bayleef", "Meganium", "Cyndaquil", "Quilava", "Typhlosion", "Totodile", "Croconaw",
			"Feraligatr", "Sentret", "Furret", "Hoothoot", "Noctowl", "Ledyba", "Ledian", "Spinarak", "Ariados", "Crobat",
			"Chinchou", "Lanturn", "Pichu", "Cleffa", "Igglybuff", "Togepi", "Togetic", "Natu", "Xatu", "Mareep",
			"Flaaffy", "Ampharos", "Bellossom", "Marill", "Azumarill", "Sudowoodo", "Politoed", "Hoppip", "Skiploom", "Jumpluff",
			"Aipom", "Sunkern", "Sunflora", "Yanma", "Wooper", "Quagsire", "Espeon", "Umbreon", "Murkrow", "Slowking",
			"Misdreavus", "Unown A", "Wobbuffet", "Girafarig", "Pineco", "Forretress", "Dunsparce", "Gligar", "Steelix", "Snubbull",
			"Granbull", "Qwilfish", "Scizor", "Shuckle", "Heracross", "Sneasel", "Teddiursa", "Ursaring", "Slugma", "Magcargo",
			"Swinub", "Piloswine", "Corsola", "Remoraid", "Octillery", "Delibird", "Mantine", "Skarmory", "Houndour", "Houndoom",
			"Kingdra", "Phanpy", "Donphan", "Porygon2", "Stantler", "Smeargle", "Tyrogue", "Hitmontop", "Smoochum", "Elekid",
			"Magby", "Miltank", "Blissey", "Raikou", "Entei", "Suicune", "Larvitar", "Pupitar", "Tyranitar", "Lugia",
			"Ho-oh", "Celebi", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)",
			"? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)",
			"? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "? (glitch Pokémon)", "Treecko", "Grovyle", "Sceptile",
			"Torchic", "Combusken", "Blaziken", "Mudkip", "Marshtomp", "Swampert", "Poochyena", "Mightyena", "Zigzagoon", "Linoone",
			"Wurmple", "Silcoon", "Beautifly", "Cascoon", "Dustox", "Lotad", "Lombre", "Ludicolo", "Seedot", "Nuzleaf",
			"Shiftry", "Nincada", "Ninjask", "Shedinja", "Taillow", "Swellow", "Shroomish", "Breloom", "Spinda", "Wingull",
			"Pelipper", "Surskit", "Masquerain", "Wailmer", "Wailord", "Skitty", "Delcatty", "Kecleon", "Baltoy", "Claydol",
			"Nosepass", "Torkoal", "Sableye", "Barboach", "Whiscash", "Luvdisc", "Corphish", "Crawdaunt", "Feebas", "Milotic",
			"Carvanha", "Sharpedo", "Trapinch", "Vibrava", "Flygon", "Makuhita", "Hariyama", "Electrike", "Manectric", "Numel",
			"Camerupt", "Spheal", "Sealeo", "Walrein", "Cacnea", "Cacturne", "Snorunt", "Glalie", "Lunatone", "Solrock",
			"Azurill", "Spoink", "Grumpig", "Plusle", "Minun", "Mawile", "Meditite", "Medicham", "Swablu", "Altaria",
			"Wynaut", "Duskull", "Dusclops", "Roselia", "Slakoth", "Vigoroth", "Slaking", "Gulpin", "Swalot", "Tropius",
			"Whismur", "Loudred", "Exploud", "Clamperl", "Huntail", "Gorebyss", "Absol", "Shuppet", "Banette", "Seviper",
			"Zangoose", "Relicanth", "Aron", "Lairon", "Aggron", "Castform", "Volbeat", "Illumise", "Lileep", "Cradily",
			"Anorith", "Armaldo", "Ralts", "Kirlia", "Gardevoir", "Bagon", "Shelgon", "Salamence", "Beldum", "Metang",
			"Metagross", "Regirock", "Regice", "Registeel", "Kyogre", "Groudon", "Rayquaza", "Latias", "Latios", "Jirachi",
			"Deoxys", "Chimecho", "Pokémon Egg", "Unown B", "Unown C", "Unown D", "Unown E", "Unown F", "Unown G", "Unown H",
			"Unown I", "Unown J", "Unown K", "Unown L", "Unown M", "Unown N", "Unown O", "Unown P", "Unown Q", "Unown R",
			"Unown S", "Unown T", "Unown U", "Unown V", "Unown W", "Unown X", "Unown Y", "Unown Z", "Unown !", "Unown ?"
		});
		this.outbreak_species.Location = new System.Drawing.Point(412, 18);
		this.outbreak_species.Name = "outbreak_species";
		this.outbreak_species.Size = new System.Drawing.Size(121, 21);
		this.outbreak_species.TabIndex = 19;
		this.label25.Location = new System.Drawing.Point(330, 122);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(78, 23);
		this.label25.TabIndex = 25;
		this.label25.Text = "Move 3";
		this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label23.Location = new System.Drawing.Point(6, 45);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(78, 23);
		this.label23.TabIndex = 22;
		this.label23.Text = "Map";
		this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label26.Location = new System.Drawing.Point(330, 96);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(78, 23);
		this.label26.TabIndex = 24;
		this.label26.Text = "Move 2";
		this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label21.Location = new System.Drawing.Point(6, 94);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(86, 23);
		this.label21.TabIndex = 24;
		this.label21.Text = "Days remaining";
		this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label27.Location = new System.Drawing.Point(330, 69);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(78, 23);
		this.label27.TabIndex = 23;
		this.label27.Text = "Move 1";
		this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.outbreak_move4.FormattingEnabled = true;
		this.outbreak_move4.Items.AddRange(new object[355]
		{
			"-NONE-", "Pound", "Karate Chop*", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch",
			"Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust*", "Wing Attack", "Whirlwind", "Fly",
			"Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack*", "Headbutt",
			"Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip",
			"Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite*", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom",
			"Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard",
			"Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss",
			"Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder",
			"Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake",
			"Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage",
			"Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray",
			"Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move",
			"Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
			"Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas",
			"Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
			"Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
			"Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web",
			"Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse*", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal",
			"Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss*", "Belly Drum", "Sludge Bomb", "Mud-Slap",
			"Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On",
			"Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm*", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark",
			"Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard",
			"Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin",
			"Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight*", "Hidden Power", "Cross Chop", "Twister",
			"Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash",
			"Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment",
			"Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt",
			"Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge",
			"Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch",
			"Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick",
			"Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash",
			"Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound",
			"Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold",
			"Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up",
			"Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance",
			"Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost"
		});
		this.outbreak_move4.Location = new System.Drawing.Point(412, 148);
		this.outbreak_move4.Name = "outbreak_move4";
		this.outbreak_move4.Size = new System.Drawing.Size(121, 21);
		this.outbreak_move4.TabIndex = 22;
		this.outbreak_map.Location = new System.Drawing.Point(114, 45);
		this.outbreak_map.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.outbreak_map.Name = "outbreak_map";
		this.outbreak_map.Size = new System.Drawing.Size(120, 20);
		this.outbreak_map.TabIndex = 19;
		this.outbreak_move3.FormattingEnabled = true;
		this.outbreak_move3.Items.AddRange(new object[355]
		{
			"-NONE-", "Pound", "Karate Chop*", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch",
			"Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust*", "Wing Attack", "Whirlwind", "Fly",
			"Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack*", "Headbutt",
			"Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip",
			"Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite*", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom",
			"Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard",
			"Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss",
			"Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder",
			"Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake",
			"Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage",
			"Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray",
			"Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move",
			"Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
			"Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas",
			"Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
			"Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
			"Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web",
			"Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse*", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal",
			"Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss*", "Belly Drum", "Sludge Bomb", "Mud-Slap",
			"Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On",
			"Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm*", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark",
			"Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard",
			"Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin",
			"Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight*", "Hidden Power", "Cross Chop", "Twister",
			"Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash",
			"Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment",
			"Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt",
			"Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge",
			"Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch",
			"Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick",
			"Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash",
			"Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound",
			"Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold",
			"Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up",
			"Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance",
			"Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost"
		});
		this.outbreak_move3.Location = new System.Drawing.Point(412, 124);
		this.outbreak_move3.Name = "outbreak_move3";
		this.outbreak_move3.Size = new System.Drawing.Size(121, 21);
		this.outbreak_move3.TabIndex = 21;
		this.label22.Location = new System.Drawing.Point(6, 72);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(78, 23);
		this.label22.TabIndex = 23;
		this.label22.Text = "Availability %";
		this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.outbreak_move2.FormattingEnabled = true;
		this.outbreak_move2.Items.AddRange(new object[355]
		{
			"-NONE-", "Pound", "Karate Chop*", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch",
			"Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust*", "Wing Attack", "Whirlwind", "Fly",
			"Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack*", "Headbutt",
			"Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip",
			"Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite*", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom",
			"Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard",
			"Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss",
			"Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder",
			"Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake",
			"Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage",
			"Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray",
			"Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move",
			"Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
			"Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas",
			"Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
			"Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
			"Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web",
			"Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse*", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal",
			"Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss*", "Belly Drum", "Sludge Bomb", "Mud-Slap",
			"Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On",
			"Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm*", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark",
			"Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard",
			"Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin",
			"Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight*", "Hidden Power", "Cross Chop", "Twister",
			"Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash",
			"Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment",
			"Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt",
			"Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge",
			"Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch",
			"Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick",
			"Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash",
			"Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound",
			"Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold",
			"Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up",
			"Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance",
			"Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost"
		});
		this.outbreak_move2.Location = new System.Drawing.Point(412, 98);
		this.outbreak_move2.Name = "outbreak_move2";
		this.outbreak_move2.Size = new System.Drawing.Size(121, 21);
		this.outbreak_move2.TabIndex = 20;
		this.outbreak_availability.Location = new System.Drawing.Point(114, 71);
		this.outbreak_availability.Maximum = new decimal(new int[4] { 255, 0, 0, 0 });
		this.outbreak_availability.Name = "outbreak_availability";
		this.outbreak_availability.Size = new System.Drawing.Size(120, 20);
		this.outbreak_availability.TabIndex = 20;
		this.outbreak_move1.FormattingEnabled = true;
		this.outbreak_move1.Items.AddRange(new object[355]
		{
			"-NONE-", "Pound", "Karate Chop*", "Double Slap", "Comet Punch", "Mega Punch", "Pay Day", "Fire Punch", "Ice Punch", "Thunder Punch",
			"Scratch", "Vice Grip", "Guillotine", "Razor Wind", "Swords Dance", "Cut", "Gust*", "Wing Attack", "Whirlwind", "Fly",
			"Bind", "Slam", "Vine Whip", "Stomp", "Double Kick", "Mega Kick", "Jump Kick", "Rolling Kick", "Sand Attack*", "Headbutt",
			"Horn Attack", "Fury Attack", "Horn Drill", "Tackle", "Body Slam", "Wrap", "Take Down", "Thrash", "Double-Edge", "Tail Whip",
			"Poison Sting", "Twineedle", "Pin Missile", "Leer", "Bite*", "Growl", "Roar", "Sing", "Supersonic", "Sonic Boom",
			"Disable", "Acid", "Ember", "Flamethrower", "Mist", "Water Gun", "Hydro Pump", "Surf", "Ice Beam", "Blizzard",
			"Psybeam", "Bubble Beam", "Aurora Beam", "Hyper Beam", "Peck", "Drill Peck", "Submission", "Low Kick", "Counter", "Seismic Toss",
			"Strength", "Absorb", "Mega Drain", "Leech Seed", "Growth", "Razor Leaf", "Solar Beam", "Poison Powder", "Stun Spore", "Sleep Powder",
			"Petal Dance", "String Shot", "Dragon Rage", "Fire Spin", "Thunder Shock", "Thunderbolt", "Thunder Wave", "Thunder", "Rock Throw", "Earthquake",
			"Fissure", "Dig", "Toxic", "Confusion", "Psychic", "Hypnosis", "Meditate", "Agility", "Quick Attack", "Rage",
			"Teleport", "Night Shade", "Mimic", "Screech", "Double Team", "Recover", "Harden", "Minimize", "Smokescreen", "Confuse Ray",
			"Withdraw", "Defense Curl", "Barrier", "Light Screen", "Haze", "Reflect", "Focus Energy", "Bide", "Metronome", "Mirror Move",
			"Self-Destruct", "Egg Bomb", "Lick", "Smog", "Sludge", "Bone Club", "Fire Blast", "Waterfall", "Clamp", "Swift",
			"Skull Bash", "Spike Cannon", "Constrict", "Amnesia", "Kinesis", "Soft-Boiled", "High Jump Kick", "Glare", "Dream Eater", "Poison Gas",
			"Barrage", "Leech Life", "Lovely Kiss", "Sky Attack", "Transform", "Bubble", "Dizzy Punch", "Spore", "Flash", "Psywave",
			"Splash", "Acid Armor", "Crabhammer", "Explosion", "Fury Swipes", "Bonemerang", "Rest", "Rock Slide", "Hyper Fang", "Sharpen",
			"Conversion", "Tri Attack", "Super Fang", "Slash", "Substitute", "Struggle", "Sketch", "Triple Kick", "Thief", "Spider Web",
			"Mind Reader", "Nightmare", "Flame Wheel", "Snore", "Curse*", "Flail", "Conversion 2", "Aeroblast", "Cotton Spore", "Reversal",
			"Spite", "Powder Snow", "Protect", "Mach Punch", "Scary Face", "Feint Attack", "Sweet Kiss*", "Belly Drum", "Sludge Bomb", "Mud-Slap",
			"Octazooka", "Spikes", "Zap Cannon", "Foresight", "Destiny Bond", "Perish Song", "Icy Wind", "Detect", "Bone Rush", "Lock-On",
			"Outrage", "Sandstorm", "Giga Drain", "Endure", "Charm*", "Rollout", "False Swipe", "Swagger", "Milk Drink", "Spark",
			"Fury Cutter", "Steel Wing", "Mean Look", "Attract", "Sleep Talk", "Heal Bell", "Return", "Present", "Frustration", "Safeguard",
			"Pain Split", "Sacred Fire", "Magnitude", "Dynamic Punch", "Megahorn", "Dragon Breath", "Baton Pass", "Encore", "Pursuit", "Rapid Spin",
			"Sweet Scent", "Iron Tail", "Metal Claw", "Vital Throw", "Morning Sun", "Synthesis", "Moonlight*", "Hidden Power", "Cross Chop", "Twister",
			"Rain Dance", "Sunny Day", "Crunch", "Mirror Coat", "Psych Up", "Extreme Speed", "Ancient Power", "Shadow Ball", "Future Sight", "Rock Smash",
			"Whirlpool", "Beat Up", "Fake Out", "Uproar", "Stockpile", "Spit Up", "Swallow", "Heat Wave", "Hail", "Torment",
			"Flatter", "Will-O-Wisp", "Memento", "Facade", "Focus Punch", "Smelling Salts", "Follow Me", "Nature Power", "Charge", "Taunt",
			"Helping Hand", "Trick", "Role Play", "Wish", "Assist", "Ingrain", "Superpower", "Magic Coat", "Recycle", "Revenge",
			"Brick Break", "Yawn", "Knock Off", "Endeavor", "Eruption", "Skill Swap", "Imprison", "Refresh", "Grudge", "Snatch",
			"Secret Power", "Dive", "Arm Thrust", "Camouflage", "Tail Glow", "Luster Purge", "Mist Ball", "Feather Dance", "Teeter Dance", "Blaze Kick",
			"Mud Sport", "Ice Ball", "Needle Arm", "Slack Off", "Hyper Voice", "Poison Fang", "Crush Claw", "Blast Burn", "Hydro Cannon", "Meteor Mash",
			"Astonish", "Weather Ball", "Aromatherapy", "Fake Tears", "Air Cutter", "Overheat", "Odor Sleuth", "Rock Tomb", "Silver Wind", "Metal Sound",
			"Grass Whistle", "Tickle", "Cosmic Power", "Water Spout", "Signal Beam", "Shadow Punch", "Extrasensory", "Sky Uppercut", "Sand Tomb", "Sheer Cold",
			"Muddy Water", "Bullet Seed", "Aerial Ace", "Icicle Spear", "Iron Defense", "Block", "Howl", "Dragon Claw", "Frenzy Plant", "Bulk Up",
			"Bounce", "Mud Shot", "Poison Tail", "Covet", "Volt Tackle", "Magical Leaf", "Water Sport", "Calm Mind", "Leaf Blade", "Dragon Dance",
			"Rock Blast", "Shock Wave", "Water Pulse", "Doom Desire", "Psycho Boost"
		});
		this.outbreak_move1.Location = new System.Drawing.Point(412, 71);
		this.outbreak_move1.Name = "outbreak_move1";
		this.outbreak_move1.Size = new System.Drawing.Size(121, 21);
		this.outbreak_move1.TabIndex = 19;
		this.label30.Location = new System.Drawing.Point(419, 523);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(235, 23);
		this.label30.TabIndex = 11;
		this.label30.Text = "Note: use Advance Map to see map values";
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(0, 523);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(242, 23);
		this.label32.TabIndex = 12;
		this.label32.Text = "Still a WIP! Make sure you have a save backup!";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(635, 539);
		base.Controls.Add(this.label32);
		base.Controls.Add(this.label30);
		base.Controls.Add(this.tv_outbreak_group);
		base.Controls.Add(this.groupBox2);
		base.Controls.Add(this.groupBox1);
		base.Controls.Add(this.save_but);
		base.Controls.Add(this.groupBox3);
		base.Name = "TV_editor";
		this.Text = "TV Program & Swarm Editor";
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.event_days).EndInit();
		((System.ComponentModel.ISupportInitialize)this.event_slot).EndInit();
		this.groupBox2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.current_remaining).EndInit();
		((System.ComponentModel.ISupportInitialize)this.current_appearance).EndInit();
		((System.ComponentModel.ISupportInitialize)this.current_map).EndInit();
		((System.ComponentModel.ISupportInitialize)this.current_level).EndInit();
		this.groupBox3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.tv_id).EndInit();
		((System.ComponentModel.ISupportInitialize)this.tv_mix_tid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.tv_tid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.tv_slot).EndInit();
		this.tv_outbreak_group.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.outbreak_activation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.outbreak_level).EndInit();
		((System.ComponentModel.ISupportInitialize)this.outbreak_remaining).EndInit();
		((System.ComponentModel.ISupportInitialize)this.outbreak_map).EndInit();
		((System.ComponentModel.ISupportInitialize)this.outbreak_availability).EndInit();
		base.ResumeLayout(false);
	}
}