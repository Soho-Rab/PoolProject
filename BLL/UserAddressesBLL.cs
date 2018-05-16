/***************************************************************************
 *
 * Copyright (c) 2018 苏州云链信息咨询有限公司 All Rights Reserved.
 * 机器名称：      DESKTOP-DF8B3JQ
 * 公司名称：      苏州云链信息咨询有限公司
 * 命名空间：      Pool.BLL
 * 文件名：        UserAddressesBLL
 * 唯一标识：      f759688c-f5d6-42aa-84de-7fc4d2eebd06
 * 当前的用户域：  DESKTOP-DF8B3JQ
 * 创建人：        DevC
 * 创建时间：      2018/3/2 16:19:24
 * 描述：          
 *
 *=====================================================================*/


using System;
using System.Collections.Generic;
using System.Text;
using Pool.Model;

namespace Pool.BLL
{
    public class UserAddressesBLL
    {
        public static int CreateUserAddresses(List<UserAddresses> addresses)
        {
            return SugarBase.DBAutoClose.Insertable<UserAddresses>(addresses).ExecuteCommand();
        }

        public static int UpdateOneUserAddresses(UserAddresses address)
        {
            return SugarBase.DBAutoClose.Updateable(address).ExecuteCommand();
        }

        public static int DelUserAddressesByIDs(int[] ids)
        {
            return SugarBase.DBAutoClose.Deleteable<UserAddresses>().In(ids).ExecuteCommand();
        }

        public static List<UserAddresses> GetAllUserAddress()
        {
            return SugarBase.DBAutoClose.Queryable<UserAddresses>().ToList();
        }

        public static int GetAllUserCount()
        {
            return SugarBase.DBAutoClose.Queryable<UserAddresses>().Count();
        }
    }
}
