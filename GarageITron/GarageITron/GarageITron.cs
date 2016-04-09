using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GarageModel;


namespace GarageITron
{
    public partial class GarageITron : Form
    {
        private GarageRepository _repo;
        private GarageMediator.GarageMediator _mediator;
        
        public GarageITron()
        {
            InitializeComponent();
            _repo = GarageRepository.Instance;
            _mediator = new GarageMediator.GarageMediator();
        }
    }
}
