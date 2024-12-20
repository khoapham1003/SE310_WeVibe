import React from "react";
import { ShoppingCartOutlined } from "@ant-design/icons";
import { Button } from "antd";
import { useNavigate } from "react-router-dom";

function CartButton() {
  const navigate = useNavigate();

  return (
    <div>
      <Button
        icon={<ShoppingCartOutlined />}
        style={{
          display: "inline",
          boxShadow: "none",
          background: "#ffffff",
          color: "#127567",
          border: "none",
          cursor: "pointer",
          width: "3rem",
        }}
        onClick={() => {
          navigate("/cart");
        }}
      ></Button>
    </div>
  );
}

export default CartButton;
