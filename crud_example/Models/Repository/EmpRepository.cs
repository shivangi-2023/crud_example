using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using crud_example.Models;
using Dapper;
using System.Linq;
using System.Configuration.Provider;

namespace crud_example.Models.Repository
{
    public class EmpRepository
    {                     
            public SqlConnection con;
            //To Handle connection related activities
            private void connection()
            
        {
            string constr = @"Data Source=DESKTOP-6BQAM4C;Initial Catalog=Userdetails_skills;Persist Security Info=True;User ID=sa;Password=123";

                   //string constr = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
                con = new SqlConnection(constr);
            }
            
        //To Add Employee details
        public void AddEmployee(EmpModel empModel)
            {
               
                try
                {

                DynamicParameters pram = new DynamicParameters();
                pram.Add("Username", empModel.Username);
                pram.Add("Password", empModel.Password);
                pram.Add("Email", empModel.Email);
                pram.Add("DOB", empModel.DOB);
                pram.Add("Gender", empModel.Gender);
               
                connection();
                con.Open();
                con.Execute("AddNewEmpDetails", pram, commandType: CommandType.StoredProcedure);
                con.Close();

            }
            catch (Exception ex)
                {
                    throw ex;
                }
            }
        public List<EmpModel> GetAllEmployees()
        {
            try
                {
                connection();
                con.Open();
                IList<EmpModel> EmpList = SqlMapper.Query<EmpModel>(con, "SELECT Id as Userid, Username, Password, Email, DOB, Gender FROM User_Info ORDER BY Username ASC").ToList();

                con.Close();
                return EmpList.ToList();
            }
            catch (Exception)
            {
                throw;
            }



        }

        //To Update Employee details
        public void UpdateEmployee(EmpModel empModel)
        {
            try
            {
                DynamicParameters pram = new DynamicParameters();
                pram.Add("Userid", empModel.Userid);
                pram.Add("Username", empModel.Username);
                pram.Add("Password", empModel.Password);
                pram.Add("Email", empModel.Email);
                pram.Add("DOB", empModel.DOB);
                pram.Add("Gender", empModel.Gender);

                connection();
                con.Open();
                con.Execute("UpdateEmpDetails", pram, commandType: CommandType.StoredProcedure);
                con.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //To delete Employee details
        public bool DeleteEmployee(int id)
        {
            try
            {
                DynamicParameters pram = new DynamicParameters();
                pram.Add("Userid", id); 
                connection();
                con.Open();
                con.Execute("DeleteEmpById", pram, commandType: CommandType.StoredProcedure);
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                //Log error as per your need 
                throw ex;
            }
        }
    }
}
