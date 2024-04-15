using Microsoft.TeamFoundation.SourceControl.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOPerations.interfaces
{
    public interface IADOOperations
    {
        Task ADOCreatePullRequestBackup();
        Task RestorePrsByDateTime();
    }
}
