import React, { useEffect, useState } from "react";
import axios from "axios";
import "../Styles/DashboardStyles.css"; 

const Dashboard = ({ token }) => {
  const [userData, setUserData] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(
          "https://localhost:7222/api/User/GetAllCustomerList",
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

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
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Index</th>
            <th>Age</th>
            <th>EyeColor</th>
            <th>Name</th>
            <th>Gender</th>
            <th>Company</th>
            <th>Email</th>
            <th>Phone</th>
            <th>About</th>
            <th>Registered</th>
            <th>Latitude</th>
            <th>Longitude</th>
            <th>Tags</th>
            <th>AddressID</th>
            <th>Number</th>
            <th>Street</th>
            <th>City</th>
            <th>State</th>
            <th>Zipcode</th>
          </tr>
        </thead>
        <tbody>
          {userData.map((user) => (
            <tr key={user.userData._id}>
              <td>{user.userData._id}</td>
              <td>{user.userData.index}</td>
              <td>{user.userData.age}</td>
              <td>{user.userData.eyeColor}</td>
              <td>{user.userData.name}</td>
              <td>{user.userData.gender}</td>
              <td>{user.userData.company}</td>
              <td>{user.userData.email}</td>
              <td>{user.userData.phone}</td>
              <td>{user.userData.about}</td>
              <td>{user.userData.registered}</td>
              <td>{user.userData.latitude}</td>
              <td>{user.userData.longitude}</td>
              <td>{user.userData.tags.join(", ")}</td>
              <td>
                {user.userData.address ? user.userData.address.addressId : ""}
              </td>
              <td>
                {user.userData.address ? user.userData.address.number : ""}
              </td>
              <td>
                {user.userData.address ? user.userData.address.street : ""}
              </td>
              <td>{user.userData.address ? user.userData.address.city : ""}</td>
              <td>
                {user.userData.address ? user.userData.address.state : ""}
              </td>
              <td>
                {user.userData.address ? user.userData.address.zipcode : ""}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default Dashboard;
