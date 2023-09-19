import React, { useState } from "react";
import axios from "axios";
import { API_Login_URL } from "../api/apihelp.js";

const Login = ({ setToken }) => {
  const [Username, setUsername] = useState("");
  const [Password, setPassword] = useState("");

  const handleLogin = async () => {
    try {
      const response = await axios.post(`${API_Login_URL}`, {
        username: Username,
        password: Password,
      });

      const token = response.data.access_token;
      setToken(token);
    } catch (error) {
      console.error("Login failed", error);
    }
  };

  return (
    <div className="login-container">
      <h2>Admin Login</h2>
      <input
        type="text"
        placeholder="Username"
        onChange={(e) => setUsername(e.target.value)}
      />
      <input
        type="password"
        placeholder="Password"
        onChange={(e) => setPassword(e.target.value)}
      />
      <button onClick={handleLogin}>Login</button>
    </div>
  );
};

export default Login;
