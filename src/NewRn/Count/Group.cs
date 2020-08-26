using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewRn
{
    class Group
    {
        #region Properties

        private int id = 0; //идентификатор группы
        public int Id
        {
            get { return id; }
        }

        private string name = ""; //название группы
        public string Name
        {
            get { return name; }
        }

        #endregion

        #region Constructor

        public Group(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        #endregion
    }
}
