import React, { useState, useEffect } from "react";
import { Bar } from "react-chartjs-2";
import { DatePicker, Space, Button, Select } from "antd";
import { Card, Row, Descriptions, Modal, Table } from "antd";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";
ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);
const { Option } = Select;

function OrderAdmin() {
  const [items, setItems] = useState([]);
  const [chartData, setChartData] = useState({});
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
  const [chartType, setChartType] = useState("daily");
  const [dataType, setDataType] = useState("totalPrice");
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [selectedOrderItems, setSelectedOrderItems] = useState([]);

  const showOrderItems = (orderItems) => {
    setSelectedOrderItems(orderItems);
    setIsModalVisible(true);
  };

  const handleModalClose = () => {
    setIsModalVisible(false);
    setSelectedOrderItems([]);
  };

  const columns = [
    {
      title: "Tên sản phẩm",
      dataIndex: ["product", "name"],
      key: "name",
    },
    {
      title: "Màu sắc",
      dataIndex: "colorName",
      key: "colorName",
    },
    {
      title: "Kích thước",
      dataIndex: "sizeName",
      key: "sizeName",
    },
    {
      title: "Số lượng",
      dataIndex: "quantity",
      key: "quantity",
      align: "center",
    },
    {
      title: "Giá",
      dataIndex: "unitPrice",
      key: "unitPrice",
      align: "right",
      render: (price) =>
        price.toLocaleString("vi-VN", { style: "currency", currency: "VND" }),
    },
  ];

  const handleConfirmOrder = async (orderId) => {
    try {
      const requestBody = {
        status: "COMPLETED",
      };
      const response = await fetch(
        `https://localhost:7180/api/Admin/${orderId}/status`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(requestBody),
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const data = await response.json();
      console.log("Order confirmed:", data);
      fetchProductData();
    } catch (error) {
      console.error("Error confirming order:", error);
    }
  };
  const getCookie = (cookieName) => {
    const cookies = document.cookie.split("; ");
    for (const cookie of cookies) {
      const [name, value] = cookie.split("=");
      if (name === cookieName) {
        return value;
      }
    }
    return null;
  };
  const userId = getCookie("userid");
  const jwtToken = getCookie("accessToken");
  useEffect(() => {
    fetchProductData();
  }, []);

  const fetchProductData = async () => {
    try {
      const response = await fetch(
        `https://localhost:7180/api/Admin/Get-All-Orders`,
        {
          headers: {
            mode: "cors",
            "Content-Type": "application/json",
          },
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      const data = await response.json();
      console.log(data);
      setItems(data);
    } catch (error) {
      console.error("Error fetching product data:", error);
      throw error;
    }
  };

  return (
    <div>
      <Row>
        <h2 className="detail_h2">Giao Dịch Gần Đây</h2>
      </Row>

      <div
        className="order-history"
        style={{ display: "flex", flexDirection: "column-reverse" }}
      >
        {items.map((item) => (
          <Card
            key={item.orderId} // Thêm key để tránh cảnh báo React
            className="order_history_cart"
            bodyStyle={{ padding: "5px 3vw 0px" }}
            hoverable
          >
            <Descriptions column={1} size="small">
              <Descriptions.Item label="Tên người nhận">
                {item.recipientName}
              </Descriptions.Item>
              <Descriptions.Item label="Địa chỉ nhận hàng">
                {item.addressValue}
              </Descriptions.Item>
              <Descriptions.Item label="Tổng đơn hàng">
                {item.totalAmount.toLocaleString("vi-VN", {
                  style: "currency",
                  currency: "VND",
                })}{" "}
              </Descriptions.Item>
              <Descriptions.Item>
                <Button
                  type="primary"
                  onClick={() => showOrderItems(item.orderItems)}
                >
                  Xem sản phẩm
                </Button>
              </Descriptions.Item>
              <Descriptions.Item>
                {item.status === "Pending" && (
                  <Button
                    type="primary"
                    onClick={() => handleConfirmOrder(item.orderId)}
                  >
                    Xác nhận đơn hàng
                  </Button>
                )}
              </Descriptions.Item>
            </Descriptions>
          </Card>
        ))}
        <Modal
          title="Chi tiết sản phẩm"
          visible={isModalVisible}
          onCancel={handleModalClose}
          footer={[
            <Button key="close" onClick={handleModalClose}>
              Đóng
            </Button>,
          ]}
        >
          <Table
            columns={columns}
            dataSource={selectedOrderItems}
            rowKey="orderItemId"
            pagination={false}
          />
        </Modal>
      </div>
    </div>
  );
}

export default OrderAdmin;
