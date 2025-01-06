using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multitenant.Core.Entities;
// الكيان الأساسي الذي سترث منه كل الكيانات
// يحتوي على خاصية Id التي ستكون موجودة في كل الكيانات
public abstract class BaseEntity
{
    public int Id { get; private set; }
}
