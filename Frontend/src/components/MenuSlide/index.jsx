import React, { useEffect, useState } from "react";
import { Menu } from "antd";
import { MenuOutlined } from "@ant-design/icons";
import { useLocation } from "react-router-dom";

const { SubMenu } = Menu;

const MenuSlide = ({ onMenuSelect }) => {
  const location = useLocation();
  const [selectedKeys, setSelectedKeys] = useState("/");
  const [menuData, setMenuData] = useState([]);
  const [menuVisible, setMenuVisible] = useState(false);

  // Transform flat data into hierarchical structure
  const transformMenuData = (data) => {
    // Mock transformation logic if needed
    const menuMap = {};
    const roots = [];

    data.forEach((item) => {
      menuMap[item.categoryId] = { ...item, children: [] };
    });

    data.forEach((item) => {
      if (item.gender !== 0) {
        // Assume 'gender' determines parent-child relationship
        menuMap[item.gender]?.children.push(menuMap[item.categoryId]);
      } else {
        roots.push(menuMap[item.categoryId]);
      }
    });

    return roots;
  };

  // Fetch menu data
  const fetchMenuData = async () => {
    try {
      const response = await fetch("https://localhost:7180/api/Category", {
        mode: "cors",
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      const data = await response.json();
      console.log("API data:", data);
      const transformedData = transformMenuData(data);
      setMenuData(transformedData);
    } catch (error) {
      console.error("Error fetching menu data:", error);
    }
  };

  useEffect(() => {
    fetchMenuData();
  }, []);

  useEffect(() => {
    setSelectedKeys(location.pathname);
  }, [location.pathname]);

  // Render menu items recursively
  const renderMenuItems = (items) => {
    return items.map((item) => {
      if (item.children && item.children.length > 0) {
        return (
          <SubMenu key={item.categoryId} title={item.name}>
            {renderMenuItems(item.children)}
          </SubMenu>
        );
      } else {
        return <Menu.Item key={item.categoryId}>{item.name}</Menu.Item>;
      }
    });
  };

  return (
    <div>
      <button onClick={() => setMenuVisible(!menuVisible)}>
        <MenuOutlined />
      </button>
      {menuVisible && (
        <Menu
          selectedKeys={[selectedKeys]}
          style={{
            backgroundColor: "#fff",
            borderRight: "none",
            fontSize: "10px",
          }}
          onClick={({ key }) => onMenuSelect(key)}
        >
          {renderMenuItems(menuData)}
        </Menu>
      )}
    </div>
  );
};

export default MenuSlide;
