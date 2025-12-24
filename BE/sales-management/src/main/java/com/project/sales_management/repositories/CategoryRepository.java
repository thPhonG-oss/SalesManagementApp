package com.project.sales_management.repositories;

import com.project.sales_management.models.Category;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface CategoryRepository extends JpaRepository<Category, Long> {
    Category findByCategoryName(String userName);
    boolean existsByCategoryName(String categoryName);
    List<Category> findAllByIsActiveTrue();

}
