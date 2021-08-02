using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api.Application.Common.Attributes
{
    /// <summary>
    /// Marks the Command/Query for authorisation via <see cref="AuthorisationBehaviour"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class AuthoriseAttribute : Attribute
    {
        public AuthoriseAttribute() { }

        /// <summary>
        /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
        /// </summary>
        public string Roles { get; set; }

        /// <summary>
        /// Gets or sets the policy name that determines access to the resource.
        /// </summary>
        public string Policy { get; set; }

        /// <summary>
        /// Gets or sets the claims determine access to the resource.
        /// </summary>
        public string[] Claims { get; set; }
    }
}
