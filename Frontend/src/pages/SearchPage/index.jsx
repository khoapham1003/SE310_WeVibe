import React, { useEffect, useState } from "react";
import { Card, Col, Rate, Row } from "antd";
import { useNavigate } from "react-router-dom";
import MenuSlide from "../../components/MenuSlide";

const SearchPage = () => {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const searchValue = localStorage.getItem("datasearch");
  const [selectedMenu, setSelectedMenu] = useState(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);

        const response = await fetch(
          `https://localhost:7180/api/Product/search?searchString=${searchValue}`
        );

        if (!response.ok) {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("API Response:", data);

        setItems(data);
      } catch (error) {
        console.error("API Error:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleMenuSelect = (selectedValue) => {
    setSelectedMenu(selectedValue);
    navigate(`/${selectedValue}`);
  };

  const handleCardClick = (item) => {
    console.log("Card clicked:", item);
    navigate(`/product-detail/${item.productId}`, { state: { item } });
  };

  return (
    <div>
      <h3 class="title-comm">
        <span class="title-holder">TÌM KIẾM SẢN PHẨM</span>
      </h3>
      <Row className="title_bar">
        <Col>
          <MenuSlide onMenuSelect={handleMenuSelect} />
        </Col>
      </Row>
      <div className="card_container">
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

export default SearchPage;
