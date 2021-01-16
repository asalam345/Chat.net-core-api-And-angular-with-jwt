﻿using System;
using System.Data.SqlClient;

namespace Repository.SqlServer
{
    public abstract class ARepository
    {
        protected SqlConnection _context;
        protected SqlTransaction _transaction;

        protected SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query, _context, _transaction);
        }
    }
}
