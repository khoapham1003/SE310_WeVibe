import {
  Button,
  Card,
  Col,
  Image,
  Row,
  Modal,
  Form,
  Input,
  InputNumber,
  Select,
  Upload,
} from "antd";
import React, { useState, useEffect } from "react";
import { FaEdit, FaTrash } from "react-icons/fa";
import { ExclamationCircleOutlined } from "@ant-design/icons";
import { IoMdAdd } from "react-icons/io";
import { UploadOutlined } from "@ant-design/icons";
import "./../../../stylePage.css";
import { FaBoxesPacking, FaRightToBracket } from "react-icons/fa6";

const { Option } = Select;

function ProductAdmin() {
  const [items, setItems] = useState([]);
  const [categories, setCategories] = useState([]);
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [isAddModalVisible, setIsAddModalVisible] = useState(false);
  const [currentItemId, setCurrentItemId] = useState(null);
  const [form] = Form.useForm();
  const [addForm] = Form.useForm();
  const [uploadedFiles, setUploadedFiles] = useState([]);
  const { confirm } = Modal;

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
        "https://localhost:7180/api/Product/products-and-categories",
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
      const data = await response.json();
      console.log(data);
      setItems(data.products);
      setCategories(data.categories);
    } catch (error) {
      console.error("Error fetching product data:", error);
    }
  };

  const removeProduct = async (ItemId) => {
    try {
      const response = await fetch(
        `https://localhost:7180/api/Product/${ItemId}`,
        {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      await fetchProductData();
    } catch (error) {
      console.error("Error removing item:", error);
    }
  };

  const handleRemoveItem = (ItemId) => {
    removeProduct(ItemId);
  };

  const editProduct = async (ItemId, requestBody) => {
    try {
      const response = await fetch(
        `https://localhost:7180/api/ProductVariant`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(requestBody),
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      await fetchProductData();
    } catch (error) {
      console.error("Error updating product:", error);
    }
  };

  const addProduct = async (requestBody) => {
    try {
      const formData = new FormData();

      // Thêm các trường dữ liệu vào FormData
      formData.append("Name", requestBody.name);
      formData.append("Slug", requestBody.slug);
      formData.append("Description", requestBody.description);
      formData.append("CategoryId", requestBody.categoryId);

      // Thêm ảnh vào FormData
      requestBody.images.forEach((image) => {
        formData.append("Images", image); // Đảm bảo `image` là một đối tượng Blob hoặc File
      });

      const response = await fetch("https://localhost:7180/api/Product", {
        method: "POST",
        headers: {
          Authorization: `Bearer ${jwtToken}`,
        },
        body: formData,
      });

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      await fetchProductData();
    } catch (error) {
      console.error("Error adding product:", error);
    }
  };

  const showEditForm = (ItemId) => {
    setCurrentItemId(ItemId);
    const item = items.find((i) => i.productId === ItemId);

    // Set lại các giá trị cho form
    form.setFieldsValue({
      productId: item.productId,
      sizeName: item.sizeName,
      colorName: item.colorName,
      colorHex: item.colorHex,
      price: item.price,
      quantity: item.quantity,
    });

    setIsEditModalVisible(true);
  };

  const handleEditOk = () => {
    form
      .validateFields()
      .then((values) => {
        editProduct(currentItemId, values);
        setIsEditModalVisible(false);
        form.resetFields();
      })
      .catch((info) => {
        console.log("Validate Failed:", info);
      });
  };

  const handleEditCancel = () => {
    setIsEditModalVisible(false);
  };

  const handleAddOk = () => {
    addForm
      .validateFields()
      .then((values) => {
        const requestBody = {
          name: values.name,
          slug: values.slug,
          description: values.description,
          categoryId: values.categoryId,
          images: uploadedFiles,
        };
        addProduct(requestBody);
        setIsAddModalVisible(false);
        addForm.resetFields();
      })
      .catch((info) => {
        console.log("Validate Failed:", info);
      });
  };

  const handleAddCancel = () => {
    setIsAddModalVisible(false);
  };

  const showConfirm = (itemId) => {
    confirm({
      title: "Bạn có chắc chắn muốn xóa sản phẩm này không?",
      icon: <ExclamationCircleOutlined />,
      content: "Thao tác này không thể hoàn tác.",
      okText: "Xác nhận",
      okType: "danger",
      cancelText: "Hủy bỏ",
      onOk() {
        handleRemoveItem(itemId);
      },
      onCancel() {
        console.log("Hủy bỏ xóa sản phẩm");
      },
    });
  };

  const handleUploadChange = ({ fileList }) => {
    setUploadedFiles(fileList.map((file) => file.originFileObj));
  };

  return (
    <div>
      <Button
        onClick={() => setIsAddModalVisible(true)}
        className="profilepage_button admin_button"
      >
        <IoMdAdd />
        <strong>THÊM MỚI SẢN PHẨM</strong>
      </Button>
      <div className="cop_cartlist_header">
        <Col md={3} offset={1}>
          <h3>Sản phẩm</h3>
        </Col>
        <Col md={4}></Col>
        <Col md={3} offset={1}>
          <h3>Số lượng</h3>
        </Col>
        <Col md={3} offset={1}>
          <h3>Loại</h3>
        </Col>
      </div>
      <div className="cop_cartlist_item">
        {items.map((item) => (
          <Card className="cop_item_cart" key={item.productId}>
            <Row align="middle">
              <Col md={2} offset={1}>
                <Image
                  style={{ height: 80, width: 80 }}
                  src={`https://localhost:7180/static${item.images[0].imagePath}`}
                  alt={item.name}
                />
              </Col>
              <Col md={4} offset={1}>
                <span>{item.name}</span>
              </Col>

              <Col md={3} offset={1}>
                <span>{item.quantity}</span>
              </Col>
              <Col md={3} offset={1}>
                <span>{item.categoryName}</span>
              </Col>
              <Col md={3} offset={1}>
                <span>
                  <Button onClick={() => showConfirm(item.productId)}>
                    <FaTrash />
                  </Button>
                  <Button onClick={() => showEditForm(item.productId)}>
                    <FaRightToBracket />
                  </Button>
                </span>
              </Col>
            </Row>
          </Card>
        ))}
      </div>
      <Modal
        title="Chỉnh sửa sản phẩm"
        visible={isEditModalVisible}
        onOk={handleEditOk}
        onCancel={handleEditCancel}
        okText="Lưu"
        cancelText="Hủy bỏ"
      >
        <Form form={form} layout="vertical" name="edit_product_form">
          <Form.Item name="productId" label="Mã sản phẩm" hidden>
            <Input />
          </Form.Item>
          <Form.Item
            name="sizeName"
            label="Tên kích thước"
            rules={[
              { required: true, message: "Vui lòng nhập tên kích thước!" },
            ]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="colorName"
            label="Tên màu sắc"
            rules={[{ required: true, message: "Vui lòng nhập tên màu sắc!" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="colorHex"
            label="Mã màu (Hex)"
            rules={[{ required: true, message: "Vui lòng nhập mã màu!" }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="price"
            label="Giá"
            rules={[{ required: true, message: "Vui lòng nhập giá sản phẩm!" }]}
          >
            <InputNumber min={0} style={{ width: "100%" }} />
          </Form.Item>
          <Form.Item
            name="quantity"
            label="Số lượng"
            rules={[
              { required: true, message: "Vui lòng nhập số lượng sản phẩm!" },
            ]}
          >
            <InputNumber min={0} style={{ width: "100%" }} />
          </Form.Item>
        </Form>
      </Modal>
      <Modal
        title="Thêm sản phẩm mới"
        visible={isAddModalVisible}
        onOk={handleAddOk}
        onCancel={handleAddCancel}
        okText="Thêm"
        cancelText="Hủy bỏ"
      >
        <Form form={addForm} layout="vertical" name="add_product_form">
          <Form.Item name="name" label="Tên sản phẩm">
            <Input placeholder="Nhập tên sản phẩm" />
          </Form.Item>
          <Form.Item name="slug" label="Slug">
            <Input placeholder="Nhập slug " />
          </Form.Item>
          <Form.Item name="description" label="Mô tả">
            <Input.TextArea rows={4} placeholder="Nhập mô tả sản phẩm " />
          </Form.Item>
          <Form.Item name="categoryId" label="Danh mục">
            <Select placeholder="Chọn danh mục" style={{ width: "100%" }}>
              {categories.map((category) => (
                <Option key={category.categoryId} value={category.categoryId}>
                  {category.name}
                </Option>
              ))}
            </Select>
          </Form.Item>
          <Form.Item name="image" label="Hình ảnh">
            <Upload
              action="/upload.do"
              listType="picture"
              fileList={uploadedFiles}
              onChange={handleUploadChange}
            >
              <Button icon={<UploadOutlined />}>Tải lên hình ảnh</Button>
            </Upload>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}

export default ProductAdmin;
