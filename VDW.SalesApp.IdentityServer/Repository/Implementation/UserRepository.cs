using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using VDW.SalesApp.IdentityServer.Models;
using VDW.SalesApp.IdentityServer.Repository.Interface;

namespace VDW.SalesApp.IdentityServer.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User> GetPasswordByUserName(string userName)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string query = "SELECT TOP 1 UC.[Contact], U.[IsActive], U.[Password] FROM tblUser U INNER JOIN tblUserContact UC ON U.Id=UC.UserId " +
                                    "WHERE UC.Contact=@usrContact";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@usrContact", userName),
                    };
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        User user = new User();
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                user = new User
                                {
                                    UserName = reader["Contact"].ToString(),
                                    Password = reader["Password"].ToString(),
                                    IsActive = Convert.ToBoolean(reader["IsActive"].ToString())
                                };
                            }
                        }
                        return user;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        public async Task<UserClaims> GetUserByOtp(string phoneNumber, string otp)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string query = "SELECT TOP 1 U.[Id], U.[Name],TV.PhoneNumber FROM tblUser U INNER JOIN tblTokenVerification TV ON U.Id=TV.UserId " +
                                    "WHERE TV.PhoneNumber=@usrContact AND TV.TokenValue=@usrOtp";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@usrContact", phoneNumber),
                        new SqlParameter("@usrOtp", otp),
                    };
                    cmd.Parameters.AddRange(parameters.ToArray());
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        UserClaims claim = new UserClaims();
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                claim = new UserClaims
                                {
                                    UserId = reader["Id"].ToString(),
                                    PhoneNumber = reader["PhoneNumber"].ToString(),
                                    Name = reader["Name"].ToString()
                                };
                            }
                        }
                        return claim;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }

            }

        }

        public async Task<UserClaims> GetUserByPassword(string phoneNumber)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    string query = "SELECT TOP 1 U.[Id], U.[Name],UC.Contact,UC.ContactTypeId FROM tblUser U INNER JOIN tblUserContact UC ON U.Id=UC.UserId " +
                                    "WHERE UC.[Contact]=@usrContact";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@usrContact", phoneNumber),
                    };
                    cmd.Parameters.AddRange(parameters.ToArray());
                    UserClaims claims = new UserClaims();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                claims = new UserClaims
                                {
                                    UserId = reader["Id"].ToString(),
                                    PhoneNumber = reader["Contact"].ToString(),
                                    Name = reader["Name"].ToString()
                                };
                            }
                        }
                        return claims;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    con.Close();
                }

            }
        }
    }
}
