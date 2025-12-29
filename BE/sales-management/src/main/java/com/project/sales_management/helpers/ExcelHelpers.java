package com.project.sales_management.helpers;
import com.project.sales_management.dtos.requests.ProductImportDTO;
import com.project.sales_management.dtos.responses.ImportError;
import com.project.sales_management.dtos.responses.ProductImportResponse;
import org.apache.poi.ss.usermodel.*;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.io.InputStream;
import java.util.Iterator;
import java.util.List;


@Component
public class ExcelHelpers {
    // define columns in excel file
    private static int CATEGORY_NAME_COL = 0;
    private static int PRODUCT_NAME_COL = 1;
    private static int DESCRIPTION_COL = 2;
    private static int AUTHOR_COL = 3;
    private static int PUBLISHER_COL = 4;
    private static int PUBLICATION_YEAR_COL = 5;
    private static int PRICE_COL = 6;
    private static int STOCK_QUANTITY_COL = 7;
    private static int MIN_STOCK_QUANTITY_COL = 8;
    private static int DISCOUNT_PERCENTAGE_COL = 9;

    public static boolean hasExcelFormat(org.springframework.web.multipart.MultipartFile file) {
        String contentType = file.getContentType();
        return contentType != null && (contentType.equals("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                || contentType.equals("application/vnd.ms-excel"));

    }

    public List<ProductImportDTO> parseExcelFile(InputStream inputStream, ProductImportResponse productImportResponse) {
        List<ProductImportDTO> productImportDTOList = new java.util.ArrayList<>();

        try(Workbook workbook = new XSSFWorkbook(inputStream)) {
            Sheet sheet = workbook.getSheetAt(0);
            Iterator<Row> rowIterator = sheet.iterator();

            int rowNumber = 0;

            while (rowIterator.hasNext()) {
                Row currentRow = rowIterator.next();
                rowNumber++;
                // Bỏ qua hàng tiêu đề
                if (rowNumber == 1) {
                    continue;
                }

                // Bỏ qua các hàng trống
                if (isRowEmpty(currentRow)) {
                    continue;
                }
                try{
                    ProductImportDTO productImportDTO = parseRow(currentRow, rowNumber, productImportResponse);
                    if(productImportDTO != null) {
                        productImportDTOList.add(productImportDTO);
                    }
                } catch (RuntimeException e) {
                    productImportResponse.getErrors().add(
                            ImportError.builder()
                                    .rowNumber(rowNumber)
                                    .fieldName("General")
                                    .errorMessage("Lỗi không xác định khi phân tích dòng: " + e.getMessage())
                                    .build()
                    );
                }
            }

        } catch (IOException e) {
            throw new RuntimeException("Lỗi đọc file Excel: " + e.getMessage());
        }
        return productImportDTOList;
    }

    // parse a row from excel file to ProductImportDTO
    public ProductImportDTO parseRow(Row row, int rowNumber, ProductImportResponse productImportResponse) {
        ProductImportDTO productImportDTO = new ProductImportDTO();
        boolean hasError = false;

        // Đọc categoryName dưới dạng String
        Cell categoryNameCell = row.getCell(CATEGORY_NAME_COL);
        if (categoryNameCell != null || getCellValueAsString(categoryNameCell).trim().isEmpty()) {
            productImportResponse.getErrors().add(
                    ImportError.builder()
                            .rowNumber(rowNumber)
                            .fieldName("Category Name")
                            .errorMessage("Category Name không được để trống")
                            .build()
            );

            hasError = true;
        }
        else {
            productImportDTO.setCategoryName(getCellValueAsString(categoryNameCell).trim());
        }

        // Đọc productName dưới dạng String
        Cell productNameCell = row.getCell(PRODUCT_NAME_COL);
        if (productNameCell == null || getCellValueAsString(productNameCell).trim().isEmpty()) {
            productImportResponse.getErrors().add(
                    ImportError.builder()
                            .rowNumber(rowNumber)
                            .fieldName("Product Name")
                            .errorMessage("Product Name không được để trống")
                            .build()
            );
            hasError = true;
        } else {
            productImportDTO.setProductName(getCellValueAsString(productNameCell).trim());
        }

        // Đọc description dưới dạng String
        Cell descriptionCell = row.getCell(DESCRIPTION_COL);
        productImportDTO.setDescription(getCellValueAsString(descriptionCell).trim());

        // Đọc author dưới dạng String
        Cell authorCell = row.getCell(AUTHOR_COL);
        if(authorCell == null || getCellValueAsString(authorCell).trim().isEmpty()) {
            productImportResponse.getErrors().add(
                    ImportError.builder()
                            .rowNumber(rowNumber)
                            .fieldName("Author")
                            .errorMessage("Author không được để trống")
                            .build()
            );
            hasError = true;
        }
        else {
            productImportDTO.setAuthor(getCellValueAsString(authorCell).trim());
        }

        // Đọc publisher dưới dạng String
        Cell publisherCell = row.getCell(PUBLISHER_COL);
        if(publisherCell == null || getCellValueAsString(publisherCell).trim().isEmpty()) {
            productImportResponse.getErrors().add(
                    ImportError.builder()
                            .rowNumber(rowNumber)
                            .fieldName("Publisher")
                            .errorMessage("Publisher không được để trống")
                            .build()
            );
            hasError = true;
        }
        else {
            productImportDTO.setPublisher(getCellValueAsString(publisherCell).trim());
        }

        // Đọc publicationYear dưới dạng Integer
        Cell publicationYearCell = row.getCell(PUBLICATION_YEAR_COL);
        if(publicationYearCell == null || getCellValueAsString(publicationYearCell).trim().isEmpty()) {
            productImportResponse.getErrors().add(
                    ImportError.builder()
                            .rowNumber(rowNumber)
                            .fieldName("Publication Year")
                            .errorMessage("Publication Year không được để trống")
                            .build()
            );
            hasError = true;
        }
        else {
            Double publicationYearDouble = getCellValueAsDouble(publicationYearCell);
            if(publicationYearDouble == null) {
                productImportResponse.getErrors().add(
                        ImportError.builder()
                                .rowNumber(rowNumber)
                                .fieldName("Publication Year")
                                .errorMessage("Publication Year phải là số nguyên")
                                .build()
                );
                hasError = true;
            }
            else {
                productImportDTO.setPublicationYear(publicationYearDouble.intValue());
            }
        }

        // Đọc price dưới dạng Double
        Cell priceCell = row.getCell(PRICE_COL);
        if(priceCell == null || getCellValueAsDouble(priceCell) == null) {
            productImportResponse.getErrors().add(
                    ImportError.builder()
                            .rowNumber(rowNumber)
                            .fieldName("Price")
                            .errorMessage("Price không được để trống và phải là số")
                            .build()
            );
            hasError = true;
        }
        else {
            productImportDTO.setPrice(getCellValueAsDouble(priceCell));
        }

        // Đọc stockQuantity dưới dạng Integer
        Cell stockQuantityCell = row.getCell(STOCK_QUANTITY_COL);
        if(stockQuantityCell == null || getCellValueAsDouble(stockQuantityCell) == null) {
            productImportResponse.getErrors().add(
                    ImportError.builder()
                            .rowNumber(rowNumber)
                            .fieldName("Stock Quantity")
                            .errorMessage("Stock Quantity không được để trống và phải là số nguyên")
                            .build()
            );
            hasError = true;
        }
        else {
            productImportDTO.setStockQuantity(getCellValueAsDouble(stockQuantityCell).intValue());
        }

        // Đọc minStockQuantity dưới dạng Integer
        Cell minStockQuantityCell = row.getCell(MIN_STOCK_QUANTITY_COL);
        if(minStockQuantityCell == null || getCellValueAsDouble(minStockQuantityCell) == null) {
            productImportResponse.getErrors().add(
                    ImportError.builder()
                            .rowNumber(rowNumber)
                            .fieldName("Min Stock Quantity")
                            .errorMessage("Min Stock Quantity không được để trống và phải là số nguyên")
                            .build()
            );
            hasError = true;
        }
        else {
            productImportDTO.setMinStockQuantity(getCellValueAsDouble(minStockQuantityCell).intValue());
        }

        // Đọc discountPercentage dưới dạng Double
        Cell discountPercentageCell = row.getCell(DISCOUNT_PERCENTAGE_COL);
        if(discountPercentageCell != null) {
            productImportDTO.setDiscountPercentage(getCellValueAsDouble(discountPercentageCell));
        }
        else {
            productImportDTO.setDiscountPercentage(0.0);
        }
        return productImportDTO;
    }

    public boolean isRowEmpty(org.apache.poi.ss.usermodel.Row row) {
        if(row == null) {
            return true;
        }
        for (int c = row.getFirstCellNum(); c < row.getLastCellNum(); c++) {
            org.apache.poi.ss.usermodel.Cell cell = row.getCell(c);
            if (cell != null && cell.getCellType() != org.apache.poi.ss.usermodel.CellType.BLANK) {
                return false;
            }
        }
        return true;
    }

    public String getCellValueAsString(org.apache.poi.ss.usermodel.Cell cell) {
        if (cell == null) {
            return "";
        }
        switch (cell.getCellType()) {
            case STRING:
                return cell.getStringCellValue();
            case NUMERIC:
                if (org.apache.poi.ss.usermodel.DateUtil.isCellDateFormatted(cell)) {
                    return cell.getDateCellValue().toString();
                } else {
                    return String.valueOf((long) cell.getNumericCellValue());
                }
            case BOOLEAN:
                return String.valueOf(cell.getBooleanCellValue());
            case FORMULA:
                return cell.getCellFormula();
            case BLANK:
            case _NONE:
            case ERROR:
            default:
                return "";
        }
    }

    public Double getCellValueAsDouble(org.apache.poi.ss.usermodel.Cell cell) {
        if (cell == null) {
            return null;
        }
        switch (cell.getCellType()) {
            case NUMERIC:
                return cell.getNumericCellValue();
            case STRING:
                try {
                    return Double.parseDouble(cell.getStringCellValue());
                } catch (NumberFormatException e) {
                    return null;
                }
            default:
                return null;
        }
    }
}
