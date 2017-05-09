namespace WebSite.Controllers
{
    public class TFSServerController : Controller
    {
        //对Project Administrator进行有效性判断：是否包含组名，是否多于2个
        private int JudgeUsers(List<string> userList)
        {
            //GlobalDefinition.Domain = "163";
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, GlobalDefinition.Domain);
            foreach (string user in userList)
            {
                Principal myObject = Principal.FindByIdentity(ctx, user);

                if ((myObject is GroupPrincipal) && (userList.Count > 2))
                {
                    return 3;
                }
                //对象是组用户
                else if (myObject is GroupPrincipal)
                {
                    return 1;
                }
                else if (userList.Count > 2)
                {
                    return 2;
                }
            }
            return 0;
        }
    
    }
}
