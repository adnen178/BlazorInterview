using Microsoft.AspNetCore.Components;
using AntDesign;
using AntDesign.TableModels;
using InterviewApp.IServices;
using InterviewApp.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;

namespace InterviewApp.Pages.GearHistory
{
    public class GearHistoryBase : ComponentBase
    {
        [Inject]
        public IGearService gearService { get; set; }
        [Parameter]
        [SupplyParameterFromQuery(Name = "gearType")]
        public string gearType { get; set; } 
        public GearViewModel[] gears;
        public bool visibleDelete = false;
        public int pageIndex = 1;
        public int pageSize = 5;
        public int total = 0;
        public List<GearType> gearTypes = new List<GearType>
        {
            new GearType() { Value=1, Name="ski"},
            new GearType() { Value=2, Name="bike"},
        };
        protected override async Task OnInitializedAsync()
        {
            gears = await gearService.GetGearHistoryAsync(gearType, pageIndex, pageSize);
            total = gears.Count();
        }

        protected async Task onChange(QueryModel<GearViewModel> queryModel)
        {
            gears = await gearService.GetGearHistoryAsync(gearType, queryModel.PageIndex, queryModel.PageSize);

            pageIndex = queryModel.PageIndex;
            pageSize = queryModel.PageSize;
            total = gears.Count();
        }

        public async Task Delete(string serialNumber)
        {
            gears = Array.FindAll(gears, x => x.SerialNumber != serialNumber);
            StateHasChanged();

        }
        /***********************Delete Modal*******************************/
        public void ShowModal(string serialNumber)
        {

            IdForDelete = serialNumber;
            _modalText = "Voulez vous vraiment supprimer cette ligne?";
            visibleDelete = true;
        }
        public bool _confirmLoading = false;
        public string IdForDelete;
        public string _modalText = "";

        public async Task HandleOk(MouseEventArgs e)
        {

            _confirmLoading = true;
            await Task.Delay(1000);
            await Delete(IdForDelete);
            visibleDelete = false;
            _confirmLoading = false;
        }

        public void HandleCancel(MouseEventArgs e)
        {
            Console.WriteLine("Clicked cancel button");
            visibleDelete = false;
            visibleEdit = false;
            visibleAdd = false;
        }
        /************************Edit Modal*******************************/
        public bool loading = false;
        public bool visibleEdit = false;
        public Form<GearViewModel> _form;
        public int selectedGearTypeValue;
        public string selectedGearTypeName;
        public GearViewModel gearEdit ;

        public void toggle(bool value) => loading = value;
        public void OnFinish(EditContext editContext) => visibleEdit = false;
        public void ShowEditModal(GearViewModel gearViewModel)
        {
            visibleEdit = true;
            gearEdit = new GearViewModel
            {
                SerialNumber = gearViewModel.SerialNumber,
                DurationPerMin = gearViewModel.DurationPerMin,
                Description = gearViewModel.Description,
                EmailClient = gearViewModel.EmailClient,
                GearTypeId = gearViewModel.GearTypeId,
                GearTypeName= gearViewModel.GearTypeName,
                Price = gearViewModel.Price,
                TechnicienUserName = gearViewModel.TechnicienUserName
            };
            selectedGearTypeValue = gearViewModel.GearTypeId;
        }
        public void HandleOkEdit(MouseEventArgs e)
        {
            //mettre à jour les données
            var index = Array.FindIndex(gears, x => x.SerialNumber == gearEdit.SerialNumber);
            if (index != -1)
            {
                gears[index] = gearEdit;
                gears[index].GearTypeId = selectedGearTypeValue;
            }
            //si le type d'équipement est modifié, il doit être éliminé
            int gtId = gearType == "ski" ? 1 : 2;
            gears = Array.FindAll(gears, x => x.GearTypeId == gtId);

            _form.Submit();
        }
        /***********************Add Modal****************************/
        public bool visibleAdd = false;
        public GearViewModel gearAdd;
        public void ShowAddModal()
        {
            visibleAdd = true;
            gearAdd = new GearViewModel();
        }
        public async Task HandleOkAdd(MouseEventArgs e)
        {
            loading = true;
            await Task.Delay(1000);
            gearAdd.GearTypeId = selectedGearTypeValue;
            gearAdd.GearTypeName = selectedGearTypeValue == 1 ? "ski" : "bike";
            if(gearType == gearAdd.GearTypeName)
                gears = gears.Append(gearAdd);
            selectedGearTypeValue = 0;
            visibleAdd = false;
            loading = false;
        }


    }
}

