import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import { Button, Card, Col, Row, Pagination, Rate } from "antd";
import { useNavigate } from "react-router-dom";
import MenuSlide from "../../components/MenuSlide";

const { Meta } = Card;

const FilteredPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const pathName = location.pathname;
  const initialSelectedMenu = pathName.substring(1);
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 100,
    total: 0,
  });
  const [selectedMenu, setSelectedMenu] = useState(initialSelectedMenu);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);

        const response = await fetch(
          `https://localhost:7180/api/Product/category/${selectedMenu}`
        );

        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        setItems(data);
      } catch (error) {
        setError(error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [selectedMenu, pagination.current, pagination.pageSize]);

  const handleMenuSelect = (selectedValue) => {
    setSelectedMenu(selectedValue);
    navigate(`/${selectedValue}`);
    window.location.reload();
  };

  const handleCardClick = (item) => {
    navigate(`/product-detail/${item.productId}`, { state: { item } });
  };

  return (
    <div>
      <h3 class="title-comm">
        <span class="title-holder">SẢN PHẨM CÙNG THỂ LOẠI</span>
      </h3>
      <Row className="title_bar">
        <Col>
          <MenuSlide onMenuSelect={handleMenuSelect} />
        </Col>
      </Row>
      <div className="card_container">
        {loading && <p>Loading...</p>}
        {error && <p>Error: {error.message}</p>}
        {items.map((item) => (
          <Card
            className="card_item"
            key={item.name}
            hoverable
            bodyStyle={{ padding: "10px 24px" }}
            cover={
              <img
                className="mp_product_item_image"
                src={`https://localhost:7180/static${item.images[0].imagePath}`}
                alt={item.name}
              />
            }
            onClick={() => handleCardClick(item)}
          >
            <div className="flex_column">
              <div className="title_start_container">
                <span className="book_title">{item.name}</span>
              </div>
            </div>
          </Card>
        ))}
      </div>
    </div>
  );
};

export default FilteredPage;
