import React, { useState, useEffect } from "react";
import {
  List,
  Card,
  Button,
  InputNumber,
  Checkbox,
  Image,
  message,
  Col,
  Row,
} from "antd";
import { useNavigate } from "react-router-dom";
import "../stylePage.css";

function CartPage() {
  const [selectedItems, setSelectedItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const [items, setItems] = useState([]);
  const [CartItems, setCartItems] = useState([]);

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
  const cartId = getCookie("CartId");

  const fetchCartData = async () => {
    try {
      if (!userId) {
        console.error("User ID not found in sessionStorage");
        return;
      }

      const response = await fetch(
        `https://localhost:7180/api/Cart/user/${userId}`,
        {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${jwtToken}`,
          },
        }
      );
      console.log(response);
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const data = await response.json();
      console.log(data);
      setItems(data);
      setCartItems(data.cartItems);
      console.log(CartItems);
    } catch (error) {
      console.error("Error fetching product data:", error);
      throw error; // Propagate the error to handle it in the calling code
    }
  };

  const removeCartItem = async (cartItemId) => {
    try {
      if (selectedItems.includes(cartItemId)) {
        // If yes, update selectedItems by removing cartItemId
        setSelectedItems((prevSelectedItems) =>
          prevSelectedItems.filter((id) => id !== cartItemId)
        );
      }
      const response = await fetch(
        `https://localhost:7180/api/Cart/remove/${cartItemId}`,
        {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${jwtToken}`,
          },
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      await fetchCartData();
    } catch (error) {
      console.error("Error removing cart item:", error);
    }
  };

  useEffect(() => {
    fetchCartData();
  }, []);

  const handleCheckout = async () => {
    try {
      if (!jwtToken) {
        console.error("JWT Token is missing or invalid");
        return;
      }
      const requestData = {
        userId: userId,
      };
      const url = "https://localhost:7180/api/Order/create-order";
      const response = await fetch(url, {
        method: "POST",
        headers: {
          "Content-Type": "application/json; charset=utf-8 ",
          Accept: "text/plain",
        },
        body: JSON.stringify(requestData),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const data = await response.json();
      console.log(data);
      await localStorage.setItem('orderId', data.orderId);
      await navigate(`/checkout`);
    } catch (error) {
      message.error("Vui lòng chọn sản phẩm bạn muốn thanh toán!");
      console.error("Error placing the order:", error);
    }
  };

  const handleQuantityChange = async (itemId, value) => {
    try {
      const requestData = {
        userId: userId,
        discount: 0,
        quantity: value,
      };
      console.log("Request Data:", requestData);
      const response = await fetch(
        `https://localhost:7180/api/Cart/update/${itemId}`,
        {
          mode: "cors",
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${jwtToken}`,
          },
          body: JSON.stringify(requestData),
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      await fetchCartData(userId);
    } catch (error) {
      message.error("Không thể đặt thêm sản phẩm này!");
      console.error("Error updating cart item quantity:", error);
    }
  };

  const handleRemoveItem = (cartItemId) => {
    removeCartItem(cartItemId);
  };

  const totalAmount = CartItems.reduce(
    (total, item) => total + item.unitPrice * item.quantity,
    0
  );

  const totalQuantity = CartItems.reduce(
    (total, item) => total + item.quantity,
    0
  );

  const totalDiscount = CartItems.reduce(
    (total, item) =>
      total +
      item.unitPrice *
        item.quantity *
        (item.productVariant.product.discount || 0),
    0
  );

  return (
    <div>
      <h3 class="title-comm">
        <span class="title-holder">GIỎ HÀNG</span>
      </h3>

      <div className="cartlist_header">
        {/* <Col md={1}>
          <Checkbox
            checked={items.length === selectedItems.length}
            onChange={handleCheckAllChange}
          ></Checkbox>
        </Col> */}
        <Col md={2}>
          <h3>Sản phẩm</h3>
        </Col>
        <Col md={8}></Col>
        <Col md={3} offset={1}>
          <h3>Đơn giá</h3>
        </Col>
        <Col md={3}>
          <h3>Số lượng</h3>
        </Col>
        <Col md={3}>
          <h3>Thành tiền</h3>
        </Col>
        <Col md={1}></Col>
      </div>
      <div className="cartlist_item">
        {CartItems.map((item) => (
          <Card className="item_cart" key={item.cartItemId}>
            <Row align="middle">
              {/* <Col md={1}>
                <Checkbox
                  checked={selectedItems.includes(item.cartItemId)}
                  onChange={(e) => handleCheckboxChange(e, item.cartItemId)}
                />
              </Col> */}
              <Col md={2}>
                <Image
                  style={{
                    height: 80,
                    width: 80,
                  }}
                  src={item.productVariant.product.picture}
                  alt={item.title}
                />
              </Col>
              <Col md={8}>
                <span>{item.productVariant.product.name} </span>
              </Col>
              <Col md={3} offset={1}>
                <span> {item.unitPrice}đ</span>
              </Col>
              <Col md={3}>
                <div className="amount_part">
                  <Button
                    className="amount_change_button"
                    onClick={() =>
                      handleQuantityChange(item.cartItemId, item.quantity + 1)
                    }
                  >
                    +
                  </Button>

                  <span style={{ margin: "0px 10px" }}>{item.quantity}</span>

                  <Button
                    className="amount_change_button"
                    onClick={() =>
                      handleQuantityChange(item.cartItemId, item.quantity - 1)
                    }
                    disabled={item.quantity === 1}
                  >
                    -
                  </Button>
                </div>
              </Col>
              <Col md={3}>
                <span className="cp_item_price">
                  {item.unitPrice * item.quantity}đ
                </span>
              </Col>
              <Col md={1}>
                <Button
                  className="cp_delete_button"
                  onClick={() => handleRemoveItem(item.cartItemId)}
                >
                  Xóa
                </Button>
              </Col>
            </Row>
          </Card>
        ))}
      </div>
      <div className="order_info_cover">
        <List>
          <List.Item>
            <h2>Thanh toán</h2>
          </List.Item>
          <List.Item>
            <span>Tổng số lượng: {totalQuantity}</span>
          </List.Item>
          <List.Item>
            <span>Tổng thanh toán: {totalAmount}đ</span>
          </List.Item>

          <List.Item>
            <Button
              size="large"
              className="cart_button"
              onClick={handleCheckout}
            >
              Mua Hàng
            </Button>
          </List.Item>
        </List>
      </div>
    </div>
  );
}

export default CartPage;
