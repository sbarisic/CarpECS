using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEdit {
    public enum ImageIndex : int {
        // Icons
        TABLE = 0,
        TABLE_GEAR,
        SCRIPT,
        APPLICATION,
        COG,
        PENCIL,
        PAGE_EDIT,

        // Aliases
        GEAR = COG,

        // Program aliases
        CATEGORY = APPLICATION,
        PROPERTY_EDIT = PAGE_EDIT,
        NODE_EDIT = COG,
    }
}
