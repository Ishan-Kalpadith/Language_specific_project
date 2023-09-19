import React, { useEffect, useState } from "react";
import axios from "axios";
import "../Styles/DashboardStyles.css";
import { API_GetCustomerList_URL } from "../api/apihelp";
import Table from "../components/Dashboard_Components/Table";

const Dashboard = ({ token }) => {
  const [userData, setUserData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(`${API_GetCustomerList_URL}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        setUserData(response.data);
      } catch (error) {
        console.error("Error fetching data", error);
      }
    };

    fetchData();
  }, [token]);

  return (
    <div className="dashboard-container">
      <h2>User Data Dashboard</h2>
      <Table userData={userData} />
    </div>
  );
};

export default Dashboard;
