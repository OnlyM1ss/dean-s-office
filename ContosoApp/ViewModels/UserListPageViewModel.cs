using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contoso.Models;

namespace Contoso.App.ViewModels
{
    public class UserListPageViewModel : BindableBase,IEditableObject
    {
        private User _model;
        public void BeginEdit()
        {
            throw new NotImplementedException();
        }

        public void CancelEdit()
        {
            throw new NotImplementedException();
        }

        public void EndEdit()
        {
            throw new NotImplementedException();
        }
    }
}
